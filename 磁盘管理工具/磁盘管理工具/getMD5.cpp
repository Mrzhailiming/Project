
#include "getMD5.h"
#include <stdio.h>
int getMd5::_s[64] = { 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7,
12, 17, 22, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20,
4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 6, 10,
15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21 };

//初始化
void getMd5::init(){
	_a = 0x67452301;
	_b = 0xefcdab89;
	_c = 0x98badcfe;
	_d = 0x10325476;
	totalByte = lastChunkByte = 0;
	for (int i = 0; i < 64; i++)
	{
		_k[i] = (int)(abs(sin(i + 1)) * pow(2, 32));
	}
}
//重置
void getMd5::reset(){
	_a = 0x67452301;
	_b = 0xefcdab89;
	_c = 0x98badcfe;
	_d = 0x10325476;
	totalByte = lastChunkByte = 0;
}

//循环左移
un_int getMd5::shift(un_int src, un_int num){
	return (src << num) | (src << (32 - num));
}

void getMd5::dealChunk(){
	/*
	F(x,y,z) = (x & y) | ((~x) & z)
	G(x,y,z) = (x & z) | ( y & (~z))
	H(x,y,z) = x ^ y ^ z
	I(x,y,z) = y ^ (x | (~z))
	*/
	int a = _a;
	int b = _b;
	int c = _c;
	int d = _d;
	int ret;
	int g;
	//处理64次
	for (int i = 0; i < 64; ++i){
		if (i >= 0 && i < 16){
			ret = (b & c) | ((~b) & d);
			g = i;
		}
		else if (i >= 16 && i < 32){
			ret = (b & d) | (c & (~d));
			g = (5 * i + 1) % 16;
		}
		else if (i >= 32 && i < 48){
			ret = b ^ c ^ d;
			g = (3 * i + 5) % 16;
		}
		else{
			ret = c ^ (b | (~d));
			g = (7 * i) % 16;
		}
		int tmp = d;
		d = c;
		c = b;
		b = b + shift((a + ret + _k[i] + _chunk[g]), _s[i]);
	}
	_a += a;
	_b += b;
	_c += c;
	_d += d;
}

//填充冗余信息
void getMd5::fillChunk(){
	//先填充一个字节
	char* p = (char*)_chunk + lastChunkByte;
	*p = (char)0x80;
	int remainNum = CHUNK_BYTE - lastChunkByte - 1;
	//如果剩余不够8个字节
	if (remainNum < 8){
		//先把剩余的都填充0
		memset(p + 1, 0, remainNum);
		dealChunk();
		//处理完最后一个chunk然后再把chunk都置为0,相当于新开辟一个chunk
		memset(_chunk, 0, CHUNK_BYTE);
	}
	else{
		memset(p + 1, 0, remainNum);
	}
	unsigned long long totalBit = totalByte;
	totalBit *= 8;
	((unsigned long long*)_chunk)[7] = totalBit;
	dealChunk();
}

//数字转换为字符串
std::string getMd5::turnStr(un_int src){
	std::string map = "0123456789abcdef";
	std::string ret;
	for (int i = 0; i < 4; ++i){
		int cur = (src >> i * 8) & 0xff;
		ret += map[cur / 16];
		ret += map[cur % 16];
	}
	return ret;
}


//获取字符串的MD5
std::string getMd5::getStrMd5(std::string str){
	reset();
	totalByte = str.size();
	lastChunkByte = totalByte % 64;
	int chunkNum = totalByte / 64;
	for (int i = 0; i < chunkNum; ++i){
		memcpy(_chunk, str.c_str(), CHUNK_BYTE);
		dealChunk();
	}
	memcpy(_chunk, str.c_str(), lastChunkByte);
	fillChunk();
	return turnStr(_a).append(turnStr(_b)).append(turnStr(_c)).append(turnStr(_d));

}
//获取文件的MD5
std::string getMd5::getFileMd5(std::string str){
	//每次调用先重置
	reset();
	FILE* f = fopen(str.c_str(), "rb");
	if (f == nullptr){
		perror("打开文件失败 :");
		return "";
	}
	fseek(f, 0, SEEK_END);
	un_int totalByte = ftell(f);
	rewind(f);
	int chunkNum = totalByte / CHUNK_BYTE;
	lastChunkByte = totalByte % CHUNK_BYTE;
	char* src = new char[totalByte];
	fread(src, 1, totalByte, f);
	for (int i = 0; i < chunkNum; ++i){
		memcpy(_chunk, src + i * CHUNK_BYTE, CHUNK_BYTE);
		dealChunk();
	}
	memcpy(_chunk, src + chunkNum * CHUNK_BYTE, lastChunkByte);
	fillChunk();
	fclose(f);
	return turnStr(_a).append(turnStr(_b)).append(turnStr(_c)).append(turnStr(_d));
}