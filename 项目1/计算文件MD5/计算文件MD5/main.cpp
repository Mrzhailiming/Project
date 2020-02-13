#include "MD5.h"
using namespace std;;





int main(){
	md5 m;
	std::string s = "abcdeaeaeaea";
	cout << "asa" << endl;
	cout << m.getStringMd5(s).c_str() << endl;

	return 0;
}