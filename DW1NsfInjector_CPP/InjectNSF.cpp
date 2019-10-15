#include <vector>
#include <string.h>
#include "Assembler.h"
#include <algorithm>
typedef unsigned char byte;
using std::vector;
using std::min;
using std::max;

vector<byte> ParseNSF(const vector<byte> &nsfData, int &initAddress, int &playAddress);

vector<byte> InjectNSF(const vector<byte> &inputData, const vector<byte> &nsfData, int songNumber)
{
	if (nsfData.size() < 0x80)
	{
		return vector<byte>();
	}
	int initAddress, playAddress;
	vector<byte> nsfMemory = ParseNSF(nsfData, initAddress, playAddress);

	vector<byte> outputData;
	outputData.resize(16 + 0x4000 * 8 + 0x4000);
	memset(&outputData[0], 0, outputData.size());

	const byte *header_in = &inputData[0];
	const byte *bank0_in = &inputData[16 + 0 * 0x4000];
	const byte *bank1_in = &inputData[16 + 1 * 0x4000];
	const byte *bank2_in = &inputData[16 + 2 * 0x4000];
	const byte *bank3_in = &inputData[16 + 3 * 0x4000];
	const byte *chr_in = &inputData[16 + 4 * 0x4000];
	const int chr_size = 0x4000;

	byte *header_out = &outputData[0];
	byte *bank0_out = &outputData[16 + 0 * 0x4000];
	byte *bank1_out = &outputData[16 + 1 * 0x4000];
	byte *bank2_out = &outputData[16 + 2 * 0x4000];
	byte *bank3_out = &outputData[16 + 3 * 0x4000];
	byte *bank4_out = &outputData[16 + 4 * 0x4000];
	byte *bank5_out = &outputData[16 + 5 * 0x4000];
	byte *bank6_out = &outputData[16 + 6 * 0x4000];
	byte *bank7_out = &outputData[16 + 7 * 0x4000];
	byte *chr_out = &outputData[16 + 8 * 0x4000];

	memcpy(header_out, header_in, 16);
	header_out[4] = 8;
	memcpy(bank0_out, bank0_in, 0x4000);
	memcpy(bank1_out, bank1_in, 0x4000);
	memcpy(bank2_out, bank2_in, 0x4000);
	memcpy(bank3_out, bank3_in, 0x4000);
	memcpy(bank4_out, &nsfMemory[0x0000], 0x4000);
	memcpy(bank5_out, &nsfMemory[0x4000], 0x4000);
	memcpy(bank6_out, bank2_in, 0x4000);
	memcpy(bank7_out, bank3_in, 0x4000);
	memcpy(chr_out, chr_in, chr_size);

	//Build the Ram Code
	byte ramCode[256];
	using namespace assembler;
	
	const int ramCodeBase = 0x100;
	const int vblanked = ramCodeBase;
	SetMemory(ramCode, ramCodeBase);

	NOP();
	Label("nmi");
	INC_ABS(vblanked);
	Label("irq");
	RTI();
	Label("main");
	//call Init function with song number
	LDA_IMM(songNumber);
	JSR(initAddress);
	//enable NMI
	LDA_IMM(0x80);
	STA_ABS(0x2000);
	Label("mainloop");
	LDA_IMM(0);
	STA_ABS(vblanked);
	Label("waitloop");
	LDA_ABS(vblanked);
	BEQ("waitloop");
	JSR(playAddress);
	JMP("mainloop");
	Label("reset");
	LDA_IMM(0xFF);
	STA_ABS(0xFFFF);
	JMP_IND(0xFFFC);
	//5th write to bank selection + 5 writes to control (A = 0)
	Label("entry");
	STA_ABS(0xE000);
	STA_ABS(0x8000);
	STA_ABS(0x8000);
	STA_ABS(0x8000);
	STA_ABS(0x8000);
	STA_ABS(0x8000);
	JMP("main");

	int entry = GetLabel("entry");
	int nmi = GetLabel("nmi");
	int irq = GetLabel("irq");
	int reset = GetLabel("reset");
	int end = GetCurrentAddress();

	int ramCodeSize = end - ramCodeBase;

	ApplyFixups();

	//set vectors for NSF bank
	SetMemory(bank5_out, 0xC000);
	SetCurrentAddress(0x10000 - 6);
	WriteWord((ushort)nmi);
	WriteWord((ushort)reset);
	WriteWord((ushort)irq);

	//build loader code
	SetMemory(bank3_out, 0x8000);
	SetCurrentAddress(0x84E8);

	SEI();
	LDA_IMM(0);
	STA_ABS(0x2000);    //disable NMI
	LDX_IMM(0xFF);
	TSX();              //reset stack
	TAX();
	Label("clrloop");
	STA_ZPG_X(0x00);
	STA_ABS_X(0x100);
	STA_ABS_X(0x200);
	STA_ABS_X(0x300);
	STA_ABS_X(0x400);
	STA_ABS_X(0x500);
	STA_ABS_X(0x600);
	STA_ABS_X(0x700);
	INX();
	BNE("clrloop");

	//disable frame IRQ
	LDA_IMM(0x40);
	STA_ABS(0x4017);
	STA_ABS(0x4015);

	//copy code to RAM
	LDX_IMM((byte)ramCodeSize);
	Label("copy_loop");
	LDA_ABS_X(0);
	AddWordFixup("ram_copy_source");
	STA_ABS_X(ramCodeBase);
	DEX();
	BPL("copy_loop");

	//prepare bankswitch
	LDA_IMM(0x02);
	STA_ABS(0xE000);
	STA_ABS(0xE000);
	LSR_A();
	STA_ABS(0xE000);
	LSR_A();
	STA_ABS(0xE000);

	//jump to RAM code and complete the bankswitch
	JMP((ushort)entry);
	Label("ram_copy_source");
	IncBin(ramCode, ramCodeSize);
	ApplyFixups();

	//Patch bank 7 for the trigger code
	SetMemory(bank7_out, 0xC000);
	SetCurrentAddress(0xCCF3);
	JMP(0x84E8);

	return outputData;
}

vector<byte> ParseNSF(const vector<byte> &nsfData, int &initAddress, int &playAddress)
{
	int loadAddress = nsfData[0x08] + nsfData[0x09] * 256;
	initAddress = nsfData[0x0A] + nsfData[0x0B] * 256;
	playAddress = nsfData[0x0C] + nsfData[0x0D] * 256;
	int extraSize = nsfData[0x7D] + nsfData[0x7E] * 256 + nsfData[0x7F] * 65536;

	vector<byte> memory;
	memory.resize(0x8000);
	memset(&memory[0], 0, 0x8000);

	int loadIndex = loadAddress - 0x8000;
	if (loadIndex < 0) loadIndex = 0;
	byte *dest = &memory[loadIndex];
	int nsfSize = (int)nsfData.size() - extraSize - 0x80;
	if (nsfSize < 0) nsfSize = 0;
	if (nsfSize > 0x8000) nsfSize = 0x8000;
	if (nsfSize > 0)
	{
		memcpy(dest, &nsfData[0x80 + extraSize], nsfSize);
	}
	return memory;
}
