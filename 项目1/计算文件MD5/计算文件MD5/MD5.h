#ifndef __MD5_H__
#define  __MD5_H__

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
	//初始化
	void init();
	//获取文件长度
	uint totalByte();
	//获取文件最后一个数据块的长度
	uint lastByte();

	//填充冗余信息
	void fillChunk(uint* chunk);
	//处理一个chunk
	void dealChunk(uint* chunk);
	//循环左移
	uint shift(uint tmp, uint shiftNumber);

	///整型转字符串,最后的MD5输出
	void turnStr(uint src);
private:
	uint _totalByte;
	uint _lastByte;
	static uint s[64];
	static uint k[64];

	uint _chunk[CHUNK_SIZE];
	uint A, B, C, D;
};


//字符串的MD5
//文件的MD5


#endif __MD5_H__