#define _CRT_SECURE_NO_WARNINGS
#include "outNumber.h"


void outNumber::init(){
	cout << "请输入前几位数字: " << endl;
	cin >> beginNum;
	cout << "请输入后几位数字: " << endl;
	cin >> endNum;
	getMidNum();
}

void outNumber::getAllNum(int len, FILE* f){
	static std::string str;
	if (len == 0){
		fwrite(( beginNum + str + endNum + "\0").c_str(), 1, 12, f);
		cout << str << endl;
		//str.resize(str.size() - 1);
		return;
	}
	--len;
	for (int i = 0; i < 10; ++i){
		str += (i + '0');
		getAllNum(len, f);
		str.resize(str.size() - 1);
	}
}

//void outNumber::getAllNum(int len, FILE* f){
//	int i = 0;
//	int test = 10;
//	std::string ret;
//	for (i = 0; i < test; ++i){
//		ret.clear();
//		ret += (i + '0');
//		for (int j = 0; j < test; ++j){
//			ret += (j + '0');
//			for (int k = 0; k < test; ++k){
//				ret += (k + '0');
//				cout << ret << endl;
//				ret.resize(ret.size() - 1);
//			}
//			ret.resize(ret.size() - 1);
//		}
//		ret.resize(ret.size() - 1);
//	}
//}

void outNumber::getMidNum(){
	int len = 11 - beginNum.size() - endNum.size();
	FILE* f = fopen("number.txt", "w");
	getAllNum(len, f);

	fclose(f);
}
