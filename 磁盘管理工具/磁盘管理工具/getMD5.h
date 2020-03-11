#pragma once
#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
#include <string>


#define CHUNK_SIZE 16
#define CHUNK_BYTE 64
typedef unsigned int un_int;


class getMd5{
public:
	getMd5(){
		init();
	}
	//初始化
	void init();
	//重置
	void reset();

	//循环左移
	un_int shift(un_int src, un_int num);

	//
	void dealChunk();

	//填充冗余信息
	void fillChunk();

	//数字转换为字符串
	std::string turnStr(un_int src);

	//获取字符串的MD5
	std::string getStrMd5(std::string str);
	//获取文件的MD5
	std::string getFileMd5(std::string str);
private:
	static int _s[64];
	int _k[64];
	
	//一个数据块
	char _chunk[CHUNK_BYTE];
	un_int _a, _b, _c, _d;
	un_int totalByte, lastChunkByte;
};
