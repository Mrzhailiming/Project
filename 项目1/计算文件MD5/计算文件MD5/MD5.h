#ifndef __MD5_H__
#define  __MD5_H__
#include <math.h>
#include <iostream>
#include <fstream>

typedef unsigned int uint;
#define CHUNK_BYTE 64//一个chunk的字节数
#define CHUNK_SIZE 16//一个chunk的长度, uint[16]
//四个位运算函数
#define  F(a,b,c) (((a) & (b))|((~a) & (c)))
#define  G(a,b,c) (((a) & (c))|((b) & (~c)))
#define  H(a,b,c) ((a) ^ (b) ^ (c))
#define  I(a,b,c) ((b) ^ ((a) | (~c)))
//ABCD初始值
#define _atemp  0x67452301;
#define _btemp  0xefcdab89;
#define _ctemp  0x98badcfe;
#define _dtemp  0x10325476;

//k[64] K[i] = floor(2^(32) * abs(sin(i + 1))) 
//s[64] 循环左移的位数 s[64] = { 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7,12, 17, 22, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20,4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 6, 10,15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21 };
//uint s[64];
class md5{
public:
	md5(){
		//初始化
		init();
	}
	//初始化
	void init();
	//重置
	void reset();

	//填充冗余信息
	void fillChunk(uint* chunk);
	//处理一个chunk
	void dealChunk(uint* chunk);
	//循环左移
	uint shift(uint tmp, uint shiftNumber);

	///整型转字符串,最后的MD5输出
	std::string turnStr(uint src);

	//
	std::string getStringMd5(const std::string& str);
	std::string getFileMd5(const char* file);
private:
	uint _totalByte;
	uint _lastByte;
	//移位
	static uint s[64];
	uint k[64];

	uint _chunk[CHUNK_SIZE];
	uint A, B, C, D;
};


//字符串的MD5
//文件的MD5


#endif //__MD5_H__