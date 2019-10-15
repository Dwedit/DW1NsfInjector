#include "InjectNSF.h"
#include <stdio.h>
#include <vector>
using std::vector;

typedef unsigned char byte;

vector<byte> LoadFile(const char *fileName)
{
	vector<byte> fileData;
	FILE *file = fopen(fileName, "rb");
	fseek(file, 0, SEEK_END);
	int fileSize = ftell(file);
	fseek(file, 0, SEEK_SET);
	fileData.resize(fileSize);
	if (fileSize > 0)
	{
		fread(&fileData[0], 1, fileSize, file);
	}
	fclose(file);
	return fileData;
}

void SaveFile(const char *fileName, const vector<byte> &fileData)
{
	FILE *file = fopen(fileName, "wb");
	if (fileData.size() > 0)
	{
		fwrite(&fileData[0], 1, fileData.size(), file);
	}
	fclose(file);
}

int main(int argc, const char **argv)
{
	if (argc < 5)
	{
		fprintf(stderr, "Syntax: <dw1.nes> <nsf_file.nsf> <songnumber> <output.nes>");
		fflush(stderr);
		return 1;
	}

	const char *nesFileName = argv[1];
	const char *nsfFileName = argv[2];
	const char *songNumberStr = argv[3];
	const char *outputFileName = argv[4];

	vector<byte> nesFile = LoadFile(nesFileName);
	vector<byte> nsfFile = LoadFile(nsfFileName);
	int songNumber = atoi(songNumberStr);
	vector<byte> outputFile = InjectNSF(nesFile, nsfFile, songNumber);
	SaveFile(outputFileName, outputFile);
	return 0;
}
