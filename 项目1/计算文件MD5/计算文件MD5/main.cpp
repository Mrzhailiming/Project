#include "MD5.h"
using namespace std;;



void testTurnStr(){
	uint num = 0x12345678;
	md5 m;
	cout << m.turnStr(num).c_str() << endl;
}

void testgetStringMd5(){
	md5 m;
	std::string s = "abcdefgi";
	cout << m.getStringMd5(s).c_str() << endl;
}

void testGetFileMd5(){
	md5 m;
	cout << m.getFileMd5("test1.txt").c_str() << endl;
	m.reset();
	cout << m.getFileMd5("test2.txt").c_str() << endl;
}





int main(){
	//testTurnStr();
	//testgetStringMd5();
	testGetFileMd5();
	return 0;
}