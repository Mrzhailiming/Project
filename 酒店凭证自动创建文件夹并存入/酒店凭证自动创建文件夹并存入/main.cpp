#include "test.h"
#define BEGIN 1
#define END 2


void bg(){
	cout << "请输入路径：";
	char path[40] = { 0 };
	myWork w(path);
	//得到所有文件
	w.getAllFile();
	//处理
	w.create_moveFile();
	cout << "********chuli成功*******" << endl;
}


void ex(){
	cout << "********退出成功*******" << endl;
}
void menu(){
	cout << "**********************" << endl;
	cout << "********1. 开始*******" << endl;
	cout << "********2. 退出*******" << endl;
	cout << "**********************" << endl;
	int flag = 1;
	int sw = 0;
	while (flag){
		
		cin >> sw;
		switch (sw){
			case BEGIN:
				bg();
			case END:
				flag = 0;
				ex();
			default:
				cout << "选项错误，请重新输入" << endl;
				continue;
		}
	}
	
}
void testGetAllFile(){
	myWork m("D:\\test");
	m.getAllFile();

	m.num_fimeName();
}



int main(){
	//menu();
	testGetAllFile();
	//testNumFile();
	return 0;
}