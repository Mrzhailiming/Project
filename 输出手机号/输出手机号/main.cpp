#include "outNumber.h"


void menu(){
	cout << "**********************妖魔鬼怪快离开***************************" << endl;
	cout << "******************让那些煞笔快快离开人世***********************" << endl;
	cout << "*****************说的就是你 --> yangShaBi**********************" << endl;
	cout << "*************输入 1 即可开始查询ShaBi的电话号码****************" << endl;
	cout << "**************开始吧!让我们终极ShaBi的罪恶一生*****************" << endl;
	cout << "******************让他们下地狱去吧, 奥利给!********************" << endl;
	cout << "***************************************************************" << endl;
}

void testOutNumber(){
	outNumber out;
	int choice = 1;
	std::string gar;
	while (choice){
		menu();
		cin >> choice;
		//刷新缓冲区
		getline(cin, gar);
		switch (choice)
		{
		case 1:
			out.init();
			break;
		case 0:
			cout << "退出成功!" << endl;
			break;
		default:
			break;
		}
	}
}


int main(){

	testOutNumber();
	return 0;
}