#pragma once
#include <iostream>
#include <stdio.h>
#include <string>
//#include <ostream>

using std::cout;
using std::cin;
using std::endl;


class outNumber{
public:

	//输入前面后面的数字
	void init();

	//生成中间 n 位
	void getMidNum();
	//计算
	void getAllNum(int len, FILE* f);
private:
	std::string beginNum;
	std::string endNum;
	std::string midNum;
	std::string retNum;
};