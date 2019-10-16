#include "Assembler.h"
#include <string>
#include <map>
#include <string.h>

namespace assembler
{
	using std::map;

	enum Instruction
	{
		_BRK = 0x00,
		_PHP = 0x08,
		_BPL = 0x10,
		_CLC = 0x18,

		_JSR = 0x20,
		_PLP = 0x28,
		_BMI = 0x30,
		_SEC = 0x38,

		_RTI = 0x40,
		_PHA = 0x48,
		_BVC = 0x50,
		_CLI = 0x58,

		_RTS = 0x60,
		_PLS = 0x68,
		_BVS = 0x70,
		_SEI = 0x78,

		//? = 0x80,
		_DEY = 0x88,
		_BCC = 0x90,
		_TYA = 0x98,

		_LDY_IMM = 0xA0,
		_TAY = 0xA8,
		_BCS = 0xB0,
		_CLV = 0xB8,

		_CPY_IMM = 0xC0,
		_INY = 0xC8,
		_BNE = 0xD0,
		_CLD = 0xD8,

		_CPX_IMM = 0xE0,
		_INX = 0xE8,
		_BEQ = 0xF0,
		_SED = 0xF8,

		_ORA_X_IND = 0x01,
		_ORA_IMM = 0x09,
		_ORA_IND_Y = 0x11,
		_ORA_ABS_Y = 0x19,

		_AND_X_IND = 0x21,
		_AND_IMM = 0x29,
		_AND_IND_Y = 0x31,
		_AND_ABS_Y = 0x39,

		_EOR_X_IND = 0x41,
		_EOR_IMM = 0x49,
		_EOR_IND_Y = 0x51,
		_EOR_ABS_Y = 0x59,

		_ADC_X_IND = 0x61,
		_ADC_IMM = 0x69,
		_ADC_IND_Y = 0x71,
		_ADC_ABS_Y = 0x79,

		_STA_X_IND = 0x81,
		//??? --- = 0x89,
		_STA_IND_Y = 0x91,
		_STA_ABS_Y = 0x99,

		_LDA_X_IND = 0xA1,
		_LDA_IMM = 0xA9,
		_LDA_IND_Y = 0xB1,
		_LDA_ABS_Y = 0xB9,

		_CMP_X_IND = 0xC1,
		_CMP_IMM = 0xC9,
		_CMP_IND_Y = 0xD1,
		_CMP_ABS_Y = 0xD9,

		_SBC_X_IND = 0xE1,
		_SBC_IMM = 0xE9,
		_SBC_IND_Y = 0xF1,
		_SBC_ABS_Y = 0xF9,

		_ORA_ZPG = 0x05,
		_ORA_ABS = 0x0D,
		_ORA_ZPG_X = 0x15,
		_ORA_ABS_X = 0x1D,

		_AND_ZPG = 0x25,
		_AND_ABS = 0x2D,
		_AND_ZPG_X = 0x35,
		_AND_ABS_X = 0x3D,

		_EOR_ZPG = 0x45,
		_EOR_ABS = 0x4D,
		_EOR_ZPG_X = 0x55,
		_EOR_ABS_X = 0x5D,

		_ADC_ZPG = 0x65,
		_ADC_ABS = 0x6D,
		_ADC_ZPG_X = 0x75,
		_ADC_ABS_X = 0x7D,

		_STA_ZPG = 0x85,
		_STA_ABS = 0x8D,
		_STA_ZPG_X = 0x95,
		_STA_ABS_X = 0x9D,

		_LDA_ZPG = 0xA5,
		_LDA_ABS = 0xAD,
		_LDA_ZPG_X = 0xB5,
		_LDA_ABS_X = 0xBD,

		_CMP_ZPG = 0xC5,
		_CMP_ABS = 0xCD,
		_CMP_ZPG_X = 0xD5,
		_CMP_ABS_X = 0xDD,

		_SBC_ZPG = 0xE5,
		_SBC_ABS = 0xED,
		_SBC_ZPG_X = 0xF5,
		_SBC_ABS_X = 0xFD,

		_ASL_ZPG = 0x06,
		_ASL_ABS = 0x0E,
		_ASL_ZPG_X = 0x16,
		_ASL_ABS_X = 0x1E,

		_ROL_ZPG = 0x26,
		_ROL_ABS = 0x2E,
		_ROL_ZPG_X = 0x36,
		_ROL_ABS_X = 0x3E,

		_LSR_ZPG = 0x46,
		_LSR_ABS = 0x4E,
		_LSR_ZPG_X = 0x56,
		_LSR_ABS_X = 0x5E,

		_ROR_ZPG = 0x66,
		_ROR_ABS = 0x6E,
		_ROR_ZPG_X = 0x76,
		_ROR_ABS_X = 0x7E,

		_STX_ZPG = 0x86,
		_STX_ABS = 0x8E,
		_STX_ZPG_Y = 0x96,
		//??? --- = 0x9E,

		_LDX_ZPG = 0xA6,
		_LDX_ABS = 0xAE,
		_LDX_ZPG_Y = 0xB6,
		_LDX_ABS_Y = 0xBE,

		_DEC_ZPG = 0xC6,
		_DEC_ABS = 0xCE,
		_DEC_ZPG_X = 0xD6,
		_DEC_ABS_X = 0xDE,

		_INC_ZPG = 0xE6,
		_INC_ABS = 0xEE,
		_INC_ZPG_X = 0xF6,
		_INC_ABS_X = 0xFE,

		//??? --- = 0x02,
		_ASL_A = 0x0A,
		//??? --- = 0x12,
		//??? --- = 0x1A,

		//??? --- = 0x22,
		_ROL_A = 0x2A,
		//??? ---   = 0x32,
		//??? ---   = 0x3A,

		//??? ---   = 0x42,
		_LSR_A = 0x4A,
		//??? ---   = 0x52,
		//??? ---   = 0x5A,

		//??? ---   = 0x62,
		_ROR_A = 0x6A,
		//??? ---   = 0x72,
		//??? ---   = 0x7A,

		//??? ---   = 0x82,
		_TXA = 0x8A,
		//??? ---   = 0x92,
		_TXS = 0x9A,

		_LDX_IMM = 0xA2,
		_TAX = 0xAA,
		//??? ---   = 0xB2,
		_TSX = 0xBA,

		//??? ---   = 0xC2,
		_DEX = 0xCA,
		//??? ---   = 0xD2,
		//??? ---   = 0xDA,

		//??? ---   = 0xE2,
		_NOP = 0xEA,
		//??? ---   = 0xF2,
		//??? ---   = 0xFA,

		//??? ---   = 0x04,
		//??? ---   = 0x0C,
		//??? ---   = 0x14,
		//??? ---   = 0x1C,

		_BIT_ZPG = 0x24,
		_BIT_ABS = 0x2C,
		//??? ---   = 0x34,
		//??? ---   = 0x3C,

		//??? ---   = 0x44,
		_JMP_ABS = 0x4C,
		//??? ---   = 0x54,
		//??? ---   = 0x5C,

		//??? ---   = 0x64,
		_JMP_IND = 0x6C,
		//??? ---   = 0x74,
		//??? ---   = 0x7C,

		_STY_ZPG = 0x84,
		_STY_ABS = 0x8C,
		_STY_ZPG_X = 0x94,
		//??? ---   = 0x9C,

		_LDY_ZPG = 0xA4,
		_LDY_ABS = 0xAC,
		_LDY_ZPG_X = 0xB4,
		_LDY_ABS_X = 0xBC,

		_CPY_ZPG = 0xC4,
		_CPY_ABS = 0xCC,
		//??? ---   = 0xD4,
		//??? ---   = 0xDC,

		_CPX_ZPG = 0xE4,
		_CPX_ABS = 0xEC,
		//??? ---   = 0xF4,
		//??? ---   = 0xFC,
	};

	static int _currentAddress;
	static int _memoryBase;
	static byte *_currentMemory;
	static map<string, int> Labels;
	static map<int, string> WordFixups;
	static map<int, string> BranchFixups;

	void SetMemory(byte *memory, int memoryBase)
	{
		_currentAddress = 0;
		_currentMemory = memory;
		_memoryBase = memoryBase;
		Labels.clear();
		WordFixups.clear();
		BranchFixups.clear();
	}

	void SetCurrentAddress(int currentAddress)
	{
		_currentAddress = currentAddress - _memoryBase;
	}
	int GetCurrentAddress()
	{
		return _currentAddress + _memoryBase;
	}
	void WriteByte(byte b)
	{
		_currentMemory[_currentAddress++] = b;
	}

	void WriteWord(ushort word)
	{
		_currentMemory[_currentAddress++] = (word & 0xFF);
		_currentMemory[_currentAddress++] = ((word >> 8) & 0xFF);
	}

	void AddBranchFixup(const string &labelName)
	{
		BranchFixups[GetCurrentAddress() - 1] = labelName;
	}

	void AddWordFixup(const string &labelName)
	{
		WordFixups[GetCurrentAddress() - 2] = labelName;
	}

	void Label(const string &labelName)
	{
		Labels[labelName] = GetCurrentAddress();
	}

	int GetLabel(const string &labelName)
	{
		map<string, int>::iterator it = Labels.find(labelName);
		if (it == Labels.end()) return -1;
		return it->second;
	}
	void ApplyFixups()
	{
		int _currentAddress = GetCurrentAddress();
		map<string, int>::iterator labelIt;
		map<int, string>::iterator fixupIt;

		for (fixupIt = WordFixups.begin(); fixupIt != WordFixups.end(); fixupIt++)
		{
			const string &labelName = fixupIt->second;
			int address = fixupIt->first;
			labelIt = Labels.find(labelName);
			if (labelIt != Labels.end())
			{
				int value = labelIt->second;
				//apply word fixup
				SetCurrentAddress(address);
				WriteWord((ushort)value);
			}
		}
		for (fixupIt = BranchFixups.begin(); fixupIt != BranchFixups.end(); fixupIt++)
		{
			const string &labelName = fixupIt->second;
			int address = fixupIt->first;
			labelIt = Labels.find(labelName);
			if (labelIt != Labels.end())
			{
				int value = labelIt->second;
				//apply branch fixup
				SetCurrentAddress(address);
				int displacement = value - (address + 1);
				WriteByte((byte)(displacement & 0xFF));
			}
		}
		SetCurrentAddress(_currentAddress);
	}
	void IncBin(const byte *data, int dataSize)
	{
		memcpy(&_currentMemory[_currentAddress], data, dataSize);
		_currentAddress += dataSize;
	}

	void BRK() { WriteByte(Instruction::_BRK); }
	void PHP() { WriteByte(Instruction::_PHP); }
	void BPL(const string &labelName) { WriteByte(Instruction::_BPL); WriteByte(0); AddBranchFixup(labelName); }
	void CLC() { WriteByte(Instruction::_CLC); }
	void JSR(ushort address) { WriteByte(Instruction::_JSR); WriteWord(address); }
	void JSR(const string &labelName) { WriteByte(Instruction::_JSR); WriteWord(0); AddWordFixup(labelName); }
	void PLP() { WriteByte(Instruction::_PLP); }
	void BMI(const string &labelName) { WriteByte(Instruction::_BMI); WriteByte(0); AddBranchFixup(labelName); }
	void SEC() { WriteByte(Instruction::_SEC); }
	void RTI() { WriteByte(Instruction::_RTI); }
	void PHA() { WriteByte(Instruction::_PHA); }
	void BVC(const string &labelName) { WriteByte(Instruction::_BVC); WriteByte(0); AddBranchFixup(labelName); }
	void CLI() { WriteByte(Instruction::_CLI); }
	void RTS() { WriteByte(Instruction::_RTS); }
	void PLS() { WriteByte(Instruction::_PLS); }
	void BVS(const string &labelName) { WriteByte(Instruction::_BVS); WriteByte(0); AddBranchFixup(labelName); }
	void SEI() { WriteByte(Instruction::_SEI); }
	void DEY() { WriteByte(Instruction::_DEY); }
	void BCC(const string &labelName) { WriteByte(Instruction::_BCC); WriteByte(0); AddBranchFixup(labelName); }
	void TYA() { WriteByte(Instruction::_TYA); }
	void LDY_IMM(byte immediate) { WriteByte(Instruction::_LDY_IMM); WriteByte(immediate); }
	void TAY() { WriteByte(Instruction::_TAY); }
	void BCS(const string &labelName) { WriteByte(Instruction::_BCS); WriteByte(0); AddBranchFixup(labelName); }
	void CLV() { WriteByte(Instruction::_CLV); }
	void CPY_IMM(byte immediate) { WriteByte(Instruction::_CPY_IMM); WriteByte(immediate); }
	void INY() { WriteByte(Instruction::_INY); }
	void BNE(const string &labelName) { WriteByte(Instruction::_BNE); WriteByte(0); AddBranchFixup(labelName); }
	void CLD() { WriteByte(Instruction::_CLD); }
	void CPX_IMM(byte immediate) { WriteByte(Instruction::_CPX_IMM); WriteByte(immediate); }
	void INX() { WriteByte(Instruction::_INX); }
	void BEQ(const string &labelName) { WriteByte(Instruction::_BEQ); WriteByte(0); AddBranchFixup(labelName); }
	void SED() { WriteByte(Instruction::_SED); }
	void ORA_X_IND(byte address) { WriteByte(Instruction::_ORA_X_IND); WriteByte(address); }
	void ORA_IMM(byte immediate) { WriteByte(Instruction::_ORA_IMM); WriteByte(immediate); }
	void ORA_IND_Y(byte address) { WriteByte(Instruction::_ORA_IND_Y); WriteByte(address); }
	void ORA_ABS_Y(ushort address) { WriteByte(Instruction::_ORA_ABS_Y); WriteWord(address); }
	void AND_X_IND(byte address) { WriteByte(Instruction::_AND_X_IND); WriteByte(address); }
	void AND_IMM(byte immediate) { WriteByte(Instruction::_AND_IMM); WriteByte(immediate); }
	void AND_IND_Y(byte address) { WriteByte(Instruction::_AND_IND_Y); WriteByte(address); }
	void AND_ABS_Y(ushort address) { WriteByte(Instruction::_AND_ABS_Y); WriteWord(address); }
	void EOR_X_IND(byte address) { WriteByte(Instruction::_EOR_X_IND); WriteByte(address); }
	void EOR_IMM(byte immediate) { WriteByte(Instruction::_EOR_IMM); WriteByte(immediate); }
	void EOR_IND_Y(byte address) { WriteByte(Instruction::_EOR_IND_Y); WriteByte(address); }
	void EOR_ABS_Y(ushort address) { WriteByte(Instruction::_EOR_ABS_Y); WriteWord(address); }
	void ADC_X_IND(byte address) { WriteByte(Instruction::_ADC_X_IND); WriteByte(address); }
	void ADC_IMM(byte immediate) { WriteByte(Instruction::_ADC_IMM); WriteByte(immediate); }
	void ADC_IND_Y(byte address) { WriteByte(Instruction::_ADC_IND_Y); WriteByte(address); }
	void ADC_ABS_Y(ushort address) { WriteByte(Instruction::_ADC_ABS_Y); WriteWord(address); }
	void STA_X_IND(byte address) { WriteByte(Instruction::_STA_X_IND); WriteByte(address); }
	void STA_IND_Y(byte address) { WriteByte(Instruction::_STA_IND_Y); WriteByte(address); }
	void STA_ABS_Y(ushort address) { WriteByte(Instruction::_STA_ABS_Y); WriteWord(address); }
	void LDA_X_IND(byte address) { WriteByte(Instruction::_LDA_X_IND); WriteByte(address); }
	void LDA_IMM(byte immediate) { WriteByte(Instruction::_LDA_IMM); WriteByte(immediate); }
	void LDA_IND_Y(byte address) { WriteByte(Instruction::_LDA_IND_Y); WriteByte(address); }
	void LDA_ABS_Y(ushort address) { WriteByte(Instruction::_LDA_ABS_Y); WriteWord(address); }
	void CMP_X_IND(byte address) { WriteByte(Instruction::_CMP_X_IND); WriteByte(address); }
	void CMP_IMM(byte immediate) { WriteByte(Instruction::_CMP_IMM); WriteByte(immediate); }
	void CMP_IND_Y(byte address) { WriteByte(Instruction::_CMP_IND_Y); WriteByte(address); }
	void CMP_ABS_Y(ushort address) { WriteByte(Instruction::_CMP_ABS_Y); WriteWord(address); }
	void SBC_X_IND(byte address) { WriteByte(Instruction::_SBC_X_IND); WriteByte(address); }
	void SBC_IMM(byte immediate) { WriteByte(Instruction::_SBC_IMM); WriteByte(immediate); }
	void SBC_IND_Y(byte address) { WriteByte(Instruction::_SBC_IND_Y); WriteByte(address); }
	void SBC_ABS_Y(ushort address) { WriteByte(Instruction::_SBC_ABS_Y); WriteWord(address); }
	void ORA_ZPG(byte address) { WriteByte(Instruction::_ORA_ZPG); WriteByte(address); }
	void ORA_ABS(ushort address) { WriteByte(Instruction::_ORA_ABS); WriteWord(address); }
	void ORA_ZPG_X(byte address) { WriteByte(Instruction::_ORA_ZPG_X); WriteByte(address); }
	void ORA_ABS_X(ushort address) { WriteByte(Instruction::_ORA_ABS_X); WriteWord(address); }
	void AND_ZPG(byte address) { WriteByte(Instruction::_AND_ZPG); WriteByte(address); }
	void AND_ABS(ushort address) { WriteByte(Instruction::_AND_ABS); WriteWord(address); }
	void AND_ZPG_X(byte address) { WriteByte(Instruction::_AND_ZPG_X); WriteByte(address); }
	void AND_ABS_X(ushort address) { WriteByte(Instruction::_AND_ABS_X); WriteWord(address); }
	void EOR_ZPG(byte address) { WriteByte(Instruction::_EOR_ZPG); WriteByte(address); }
	void EOR_ABS(ushort address) { WriteByte(Instruction::_EOR_ABS); WriteWord(address); }
	void EOR_ZPG_X(byte address) { WriteByte(Instruction::_EOR_ZPG_X); WriteByte(address); }
	void EOR_ABS_X(ushort address) { WriteByte(Instruction::_EOR_ABS_X); WriteWord(address); }
	void ADC_ZPG(byte address) { WriteByte(Instruction::_ADC_ZPG); WriteByte(address); }
	void ADC_ABS(ushort address) { WriteByte(Instruction::_ADC_ABS); WriteWord(address); }
	void ADC_ZPG_X(byte address) { WriteByte(Instruction::_ADC_ZPG_X); WriteByte(address); }
	void ADC_ABS_X(ushort address) { WriteByte(Instruction::_ADC_ABS_X); WriteWord(address); }
	void STA_ZPG(byte address) { WriteByte(Instruction::_STA_ZPG); WriteByte(address); }
	void STA_ABS(ushort address) { WriteByte(Instruction::_STA_ABS); WriteWord(address); }
	void STA_ZPG_X(byte address) { WriteByte(Instruction::_STA_ZPG_X); WriteByte(address); }
	void STA_ABS_X(ushort address) { WriteByte(Instruction::_STA_ABS_X); WriteWord(address); }
	void LDA_ZPG(byte address) { WriteByte(Instruction::_LDA_ZPG); WriteByte(address); }
	void LDA_ABS(ushort address) { WriteByte(Instruction::_LDA_ABS); WriteWord(address); }
	void LDA_ZPG_X(byte address) { WriteByte(Instruction::_LDA_ZPG_X); WriteByte(address); }
	void LDA_ABS_X(ushort address) { WriteByte(Instruction::_LDA_ABS_X); WriteWord(address); }
	void CMP_ZPG(byte address) { WriteByte(Instruction::_CMP_ZPG); WriteByte(address); }
	void CMP_ABS(ushort address) { WriteByte(Instruction::_CMP_ABS); WriteWord(address); }
	void CMP_ZPG_X(byte address) { WriteByte(Instruction::_CMP_ZPG_X); WriteByte(address); }
	void CMP_ABS_X(ushort address) { WriteByte(Instruction::_CMP_ABS_X); WriteWord(address); }
	void SBC_ZPG(byte address) { WriteByte(Instruction::_SBC_ZPG); WriteByte(address); }
	void SBC_ABS(ushort address) { WriteByte(Instruction::_SBC_ABS); WriteWord(address); }
	void SBC_ZPG_X(byte address) { WriteByte(Instruction::_SBC_ZPG_X); WriteByte(address); }
	void SBC_ABS_X(ushort address) { WriteByte(Instruction::_SBC_ABS_X); WriteWord(address); }
	void ASL_ZPG(byte address) { WriteByte(Instruction::_ASL_ZPG); WriteByte(address); }
	void ASL_ABS(ushort address) { WriteByte(Instruction::_ASL_ABS); WriteWord(address); }
	void ASL_ZPG_X(byte address) { WriteByte(Instruction::_ASL_ZPG_X); WriteByte(address); }
	void ASL_ABS_X(ushort address) { WriteByte(Instruction::_ASL_ABS_X); WriteWord(address); }
	void ROL_ZPG(byte address) { WriteByte(Instruction::_ROL_ZPG); WriteByte(address); }
	void ROL_ABS(ushort address) { WriteByte(Instruction::_ROL_ABS); WriteWord(address); }
	void ROL_ZPG_X(byte address) { WriteByte(Instruction::_ROL_ZPG_X); WriteByte(address); }
	void ROL_ABS_X(ushort address) { WriteByte(Instruction::_ROL_ABS_X); WriteWord(address); }
	void LSR_ZPG(byte address) { WriteByte(Instruction::_LSR_ZPG); WriteByte(address); }
	void LSR_ABS(ushort address) { WriteByte(Instruction::_LSR_ABS); WriteWord(address); }
	void LSR_ZPG_X(byte address) { WriteByte(Instruction::_LSR_ZPG_X); WriteByte(address); }
	void LSR_ABS_X(ushort address) { WriteByte(Instruction::_LSR_ABS_X); WriteWord(address); }
	void ROR_ZPG(byte address) { WriteByte(Instruction::_ROR_ZPG); WriteByte(address); }
	void ROR_ABS(ushort address) { WriteByte(Instruction::_ROR_ABS); WriteWord(address); }
	void ROR_ZPG_X(byte address) { WriteByte(Instruction::_ROR_ZPG_X); WriteByte(address); }
	void ROR_ABS_X(ushort address) { WriteByte(Instruction::_ROR_ABS_X); WriteWord(address); }
	void STX_ZPG(byte address) { WriteByte(Instruction::_STX_ZPG); WriteByte(address); }
	void STX_ABS(ushort address) { WriteByte(Instruction::_STX_ABS); WriteWord(address); }
	void STX_ZPG_Y(byte address) { WriteByte(Instruction::_STX_ZPG_Y); WriteByte(address); }
	void LDX_ZPG(byte address) { WriteByte(Instruction::_LDX_ZPG); WriteByte(address); }
	void LDX_ABS(ushort address) { WriteByte(Instruction::_LDX_ABS); WriteWord(address); }
	void LDX_ZPG_Y(byte address) { WriteByte(Instruction::_LDX_ZPG_Y); WriteByte(address); }
	void LDX_ABS_Y(ushort address) { WriteByte(Instruction::_LDX_ABS_Y); WriteWord(address); }
	void DEC_ZPG(byte address) { WriteByte(Instruction::_DEC_ZPG); WriteByte(address); }
	void DEC_ABS(ushort address) { WriteByte(Instruction::_DEC_ABS); WriteWord(address); }
	void DEC_ZPG_X(byte address) { WriteByte(Instruction::_DEC_ZPG_X); WriteByte(address); }
	void DEC_ABS_X(ushort address) { WriteByte(Instruction::_DEC_ABS_X); WriteWord(address); }
	void INC_ZPG(byte address) { WriteByte(Instruction::_INC_ZPG); WriteByte(address); }
	void INC_ABS(ushort address) { WriteByte(Instruction::_INC_ABS); WriteWord(address); }
	void INC_ZPG_X(byte address) { WriteByte(Instruction::_INC_ZPG_X); WriteByte(address); }
	void INC_ABS_X(ushort address) { WriteByte(Instruction::_INC_ABS_X); WriteWord(address); }
	void ASL_A() { WriteByte(Instruction::_ASL_A); }
	void ROL_A() { WriteByte(Instruction::_ROL_A); }
	void LSR_A() { WriteByte(Instruction::_LSR_A); }
	void ROR_A() { WriteByte(Instruction::_ROR_A); }
	void TXA() { WriteByte(Instruction::_TXA); }
	void TXS() { WriteByte(Instruction::_TXS); }
	void LDX_IMM(byte immediate) { WriteByte(Instruction::_LDX_IMM); WriteByte(immediate); }
	void TAX() { WriteByte(Instruction::_TAX); }
	void TSX() { WriteByte(Instruction::_TSX); }
	void DEX() { WriteByte(Instruction::_DEX); }
	void NOP() { WriteByte(Instruction::_NOP); }
	void BIT_ZPG(byte address) { WriteByte(Instruction::_BIT_ZPG); WriteByte(address); }
	void BIT_ABS(ushort address) { WriteByte(Instruction::_BIT_ABS); WriteWord(address); }
	void JMP(ushort address) { WriteByte(Instruction::_JMP_ABS); WriteWord(address); }
	void JMP(const string &labelName) { WriteByte(Instruction::_JMP_ABS); WriteWord(0); AddWordFixup(labelName); }
	void JMP_IND(ushort address) { WriteByte(Instruction::_JMP_IND); WriteWord(address); }
	void STY_ZPG(byte address) { WriteByte(Instruction::_STY_ZPG); WriteByte(address); }
	void STY_ABS(ushort address) { WriteByte(Instruction::_STY_ABS); WriteWord(address); }
	void STY_ZPG_X(byte address) { WriteByte(Instruction::_STY_ZPG_X); WriteByte(address); }
	void LDY_ZPG(byte address) { WriteByte(Instruction::_LDY_ZPG); WriteByte(address); }
	void LDY_ABS(ushort address) { WriteByte(Instruction::_LDY_ABS); WriteWord(address); }
	void LDY_ZPG_X(byte address) { WriteByte(Instruction::_LDY_ZPG_X); WriteByte(address); }
	void LDY_ABS_X(ushort address) { WriteByte(Instruction::_LDY_ABS_X); WriteWord(address); }
	void CPY_ZPG(byte address) { WriteByte(Instruction::_CPY_ZPG); WriteByte(address); }
	void CPY_ABS(ushort address) { WriteByte(Instruction::_CPY_ABS); WriteWord(address); }
	void CPX_ZPG(byte address) { WriteByte(Instruction::_CPX_ZPG); WriteByte(address); }
	void CPX_ABS(ushort address) { WriteByte(Instruction::_CPX_ABS); WriteWord(address); }
}
