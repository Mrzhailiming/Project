#include "getMD5.h"




void testTurnStr(){
	getMd5 m;
	un_int num = 0x12345678;
	std::cout << m.turnStr(num) << std::endl;
}

void testGetStrMd5(){
	getMd5 m;
	std::string str1 = "asdasfasfasfc";
	std::string str2 = "asdasfasfasfa";
	std::cout << m.getStrMd5(str1) << std::endl;
	std::cout << m.getStrMd5(str2) << std::endl;
}

void testGetFileMd5(){
	getMd5 m;
	//02468aceb2f828ef277227dc9fe9f155
	std::string path = "t.txt";
	std::cout << m.getFileMd5(path) << std::endl;
}



int main(){
	//testTurnStr();
	//testGetStrMd5();
	testGetFileMd5();
	return 0;
}