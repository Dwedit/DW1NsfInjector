#ifndef __INJECTNSF_H__
#define __INJECTNSF_H__

#include <vector>

typedef unsigned char byte;
using std::vector;

vector<byte> ParseNSF(const vector<byte> &nsfData, int &initAddress, int &playAddress);
vector<byte> InjectNSF(const vector<byte> &inputData, const vector<byte> &nsfData, int songNumber);

#endif
