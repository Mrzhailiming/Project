#include "MD5.h"
#include "searchFile.h"
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

void testSearchFile(){
	const string path = ".";
	vector<string> files;
	search(path, files);
	for (const auto& e : files){
		cout << e << endl;
	}
}


int main(){
	//testTurnStr();
	//testgetStringMd5();
	//testGetFileMd5();
	testSearchFile();
	return 0;
}