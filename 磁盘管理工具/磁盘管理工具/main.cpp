#include "getMD5.h"
#include "FileManager.h"



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
	std::string path = "t1.txt";
	std::cout << m.getFileMd5(path) << std::endl;
}

enum {
	EXIT,
	SCANNERFILE,
	SHOWALLFILES,
	SHOWSAMEFILES,
	DELFILEBYNAME,
	DEFILEBYMD5,
	DELSNAMEFILE,
	SHOWMD5FILE,
	SHOWFILEMD5,
	GETFILEMD5,
	GETSTRINGMD5

};

void menu(){
	std::cout << "****************************************************" << std::endl;
	std::cout << "***1 扫盘 2 显示所有文件 3 显示所有重复文件*********" << std::endl;
	std::cout << "***4 以文件名删除 5 以MD5删除 6 删除重复文件 *******" << std::endl;
	std::cout << "***7 以md5-file展示 8 以file- md5展示***************" << std::endl;
	std::cout << "***9 计算文件MD5 10 计算字符串MD5*******************" << std::endl;
	std::cout << "****************************************************" << std::endl;

}
void testTool(){
	FileManager tool;
	int choice;
	std::string gar;
	do{
		menu();
		std::cout << "请输入选项 :" << std::endl;
		std::cin >> choice;
		//刷新缓冲区
		getline(std::cin, gar);
		switch (choice){
		case SCANNERFILE:
			tool.init();
			break;
		case SHOWALLFILES:
			tool.showAllFile();
			break;
		case SHOWSAMEFILES:
			tool.showSameFile();
			break;
		case DELFILEBYNAME:
			tool.delFileByName();
			break;
		case DEFILEBYMD5:
			tool.delFileByMd5();
			break;
		case DELSNAMEFILE:
			tool.delSameFile();
			break;
		case SHOWMD5FILE:
			tool.showMd5_Name();
			break;
		case SHOWFILEMD5:
			tool.showName_Md5();
			break;
		case EXIT:
			std::cout << "退出成功" << std::endl;
			break;
		case GETFILEMD5:
			tool.getFILEmd5();
			break;
		case GETSTRINGMD5:
			tool.getSTRINGmd5();
		default:
			break;
		}
	} while (choice != EXIT);
}

int main(){
	//testTurnStr();
	//testGetStrMd5();
	//testGetFileMd5();
	testTool();
	return 0;
}