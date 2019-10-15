#ifndef __ASSEMBLER_H__
#define __ASSEMBLER_H__
#include <string>

//Brief documentation:
//In your function, add "using namespace assembler", then you can make calls.
//First Call SetMemory, and SetCurrentAddress
//Calling SetMemory will also wipe the symbol tables.
//Call ASM instructions to emit the code, such as LDA_IMM, or STA_ABS_X.
//Call Label to create labels
//Call GetLabel to get the address of a label
//Call ApplyFixups at the end to set the values of labels for Jumps and Branches

namespace assembler
{
	typedef unsigned char byte;
	typedef unsigned short ushort;
	using std::string;

	void SetMemory(byte *memory, int memoryBase);
	void SetCurrentAddress(int currentAddress);
	int GetCurrentAddress();
	void WriteByte(byte b);
	void WriteWord(ushort word);
	void AddBranchFixup(const string &labelName);
	void AddWordFixup(const string &labelName);
	void Label(const string &labelName);
	int GetLabel(const string &labelName);
	void ApplyFixups();
	void IncBin(const byte *data, int dataSize);
	void BRK();
	void PHP();
	void BPL(const string &labelName);
	void CLC();
	void JSR(ushort address);
	void JSR(const string &labelName);
	void PLP();
	void BMI(const string &labelName);
	void SEC();
	void RTI();
	void PHA();
	void BVC(const string &labelName);
	void CLI();
	void RTS();
	void PLS();
	void BVS(const string &labelName);
	void SEI();
	void DEY();
	void BCC(const string &labelName);
	void TYA();
	void LDY_IMM(byte immediate);
	void TAY();
	void BCS(const string &labelName);
	void CLV();
	void CPY_IMM(byte immediate);
	void INY();
	void BNE(const string &labelName);
	void CLD();
	void CPX_IMM(byte immediate);
	void INX();
	void BEQ(const string &labelName);
	void SED();
	void ORA_X_IND(byte address);
	void ORA_IMM(byte immediate);
	void ORA_IND_Y(byte address);
	void ORA_ABS_Y(ushort address);
	void AND_X_IND(byte address);
	void AND_IMM(byte immediate);
	void AND_IND_Y(byte address);
	void AND_ABS_Y(ushort address);
	void EOR_X_IND(byte address);
	void EOR_IMM(byte immediate);
	void EOR_IND_Y(byte address);
	void EOR_ABS_Y(ushort address);
	void ADC_X_IND(byte address);
	void ADC_IMM(byte immediate);
	void ADC_IND_Y(byte address);
	void ADC_ABS_Y(ushort address);
	void STA_X_IND(byte address);
	void STA_IND_Y(byte address);
	void STA_ABS_Y(ushort address);
	void LDA_X_IND(byte address);
	void LDA_IMM(byte immediate);
	void LDA_IND_Y(byte address);
	void LDA_ABS_Y(ushort address);
	void CMP_X_IND(byte address);
	void CMP_IMM(byte immediate);
	void CMP_IND_Y(byte address);
	void CMP_ABS_Y(ushort address);
	void SBC_X_IND(byte address);
	void SBC_IMM(byte immediate);
	void SBC_IND_Y(byte address);
	void SBC_ABS_Y(ushort address);
	void ORA_ZPG(byte address);
	void ORA_ABS(ushort address);
	void ORA_ZPG_X(byte address);
	void ORA_ABS_X(ushort address);
	void AND_ZPG(byte address);
	void AND_ABS(ushort address);
	void AND_ZPG_X(byte address);
	void AND_ABS_X(ushort address);
	void EOR_ZPG(byte address);
	void EOR_ABS(ushort address);
	void EOR_ZPG_X(byte address);
	void EOR_ABS_X(ushort address);
	void ADC_ZPG(byte address);
	void ADC_ABS(ushort address);
	void ADC_ZPG_X(byte address);
	void ADC_ABS_X(ushort address);
	void STA_ZPG(byte address);
	void STA_ABS(ushort address);
	void STA_ZPG_X(byte address);
	void STA_ABS_X(ushort address);
	void LDA_ZPG(byte address);
	void LDA_ABS(ushort address);
	void LDA_ZPG_X(byte address);
	void LDA_ABS_X(ushort address);
	void CMP_ZPG(byte address);
	void CMP_ABS(ushort address);
	void CMP_ZPG_X(byte address);
	void CMP_ABS_X(ushort address);
	void SBC_ZPG(byte address);
	void SBC_ABS(ushort address);
	void SBC_ZPG_X(byte address);
	void SBC_ABS_X(ushort address);
	void ASL_ZPG(byte address);
	void ASL_ABS(ushort address);
	void ASL_ZPG_X(byte address);
	void ASL_ABS_X(ushort address);
	void ROL_ZPG(byte address);
	void ROL_ABS(ushort address);
	void ROL_ZPG_X(byte address);
	void ROL_ABS_X(ushort address);
	void LSR_ZPG(byte address);
	void LSR_ABS(ushort address);
	void LSR_ZPG_X(byte address);
	void LSR_ABS_X(ushort address);
	void ROR_ZPG(byte address);
	void ROR_ABS(ushort address);
	void ROR_ZPG_X(byte address);
	void ROR_ABS_X(ushort address);
	void STX_ZPG(byte address);
	void STX_ABS(ushort address);
	void STX_ZPG_Y(byte address);
	void LDX_ZPG(byte address);
	void LDX_ABS(ushort address);
	void LDX_ZPG_Y(byte address);
	void LDX_ABS_Y(ushort address);
	void DEC_ZPG(byte address);
	void DEC_ABS(ushort address);
	void DEC_ZPG_X(byte address);
	void DEC_ABS_X(ushort address);
	void INC_ZPG(byte address);
	void INC_ABS(ushort address);
	void INC_ZPG_X(byte address);
	void INC_ABS_X(ushort address);
	void ASL_A();
	void ROL_A();
	void LSR_A();
	void ROR_A();
	void TXA();
	void TXS();
	void LDX_IMM(byte immediate);
	void TAX();
	void TSX();
	void DEX();
	void NOP();
	void BIT_ZPG(byte address);
	void BIT_ABS(ushort address);
	void JMP(ushort address);
	void JMP(const string &labelName);
	void JMP_IND(ushort address);
	void STY_ZPG(byte address);
	void STY_ABS(ushort address);
	void STY_ZPG_X(byte address);
	void LDY_ZPG(byte address);
	void LDY_ABS(ushort address);
	void LDY_ZPG_X(byte address);
	void LDY_ABS_X(ushort address);
	void CPY_ZPG(byte address);
	void CPY_ABS(ushort address);
	void CPX_ZPG(byte address);
	void CPX_ABS(ushort address);
}
#endif
