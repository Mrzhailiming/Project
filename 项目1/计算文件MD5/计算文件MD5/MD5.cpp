#include "MD5.h"
#include <math.h>
#include <iostream>


//以字节为单位
uint totalByte;//总共的字节数
uint lastByte;//最后一个数据块的字节数
uint A;
uint B;
uint C;
uint D;
//16 * 4 字节
uint chunk[CHUNK_SIZE];



void md5::init(){
	//初始化ABCD
	A = _atemp;
	B = _btemp;
	C = _ctemp;
	D = _dtemp;
	//长度
	_totalByte = _lastByte = 0;
	//初始化s[i],k[i]
	
	for (int i = 0; i < 64; ++i){
		k[i] = (uint)(abs(sin(i + 1.0)) * pow(2.0, 32));
	}
	s[64] = { 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 5, 9, 14, 20, 5, 9, 14,
		20, 5, 9, 14, 20, 5, 9, 14, 20, 4, 11, 16, 23,
		4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 6, 10,
		15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21 };
}

//循环移位
uint md5::shift(uint tmp, uint shiftNumber){
	return (tmp << shiftNumber) | (tmp >> (32 - shiftNumber));
}

//一个chunk的处理
void md5::dealChunk(uint* chunk){
	uint a = A;
	uint b = B;
	uint c = C;
	uint d = D;
	int f, g;
	for (int i = 0; i < 64; ++i){
		if (i >= 0 && i < 16){
			f = F(b, c, d);
			g = i;
		}
		else if (i >= 16 && i < 32){
			f = G(b, c, d);
			g = (5 * i + 1) % 16;
		}
		else if (i >= 32 && i < 48){
			f = H(b, c, d);
			g = (3 * i + 5) % 16;
		}
		else{
			f = I(b, c, d);
			g = (7 * i) % 16;
		}
		uint tmp = d;
		c = b;
		b = b + shift((a + f + k[i] + chunk[g]), s[i]);
		a = d;
	}
	//更新ABCD
	A += a;
	B += b;
	C += c;
	D += d;
}

//填充冗余信息 
void md5::fillChunk(uint* chunk){
	//先填一个字节,如果剩余的不够64bit(8字节)
	//填充冗余信息后,处理当前的chunk
	//则构建一个512bit(64字节)的数据块

	//待填充的第一个字节的位置
	char* p = (char*)chunk + _lastByte;
	*p = (char)0x80;
	++p;
	//填充1字节后,剩余的字节数
	int remainNum = CHUNK_BYTE - _lastByte - 1;
	//剩余不够64bit
	if (remainNum < 8){
		//把剩余填充0
		memset(p, 0, remainNum * 8);
		//处理chunk
		dealChunk(chunk);
		//把当前chunk全部置为0,当做新开的chunk
		memset(chunk, 0, CHUNK_BYTE * 8);
	}
	else{
		memset(p, 0, remainNum * 4);
	}
	dealChunk(chunk);
}
