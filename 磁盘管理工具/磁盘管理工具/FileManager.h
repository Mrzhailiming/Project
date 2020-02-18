#pragma once
#include "getMD5.h"
#include <unordered_set>
#include <unordered_map>
class FileManager{
public:
	void init();
	//扫盘
	void scannerFile(const std::string& path);

	//计算每一个文件的md5,并放入对应的容器中
	void getAllFilesMd5();
	void getSameFiles();

	//计算文件MD5
	void getFILEmd5();
	void getSTRINGmd5();

	//展示
	void showAllFile();
	void showSameFile();
	void showName_Md5();
	void showMd5_Name();

	//删除
	void delFileByName();
	void delFileByMd5();
	void delSameFile();
private:
	getMd5 _gMd5;
	//存放所有文件
	std::unordered_set<std::string> _allFiles;

	//存放文件名映射md5
	std::unordered_map<std::string, std::string> _name_md5;
	//存放重复文件,md5映射文件名
	std::unordered_multimap<std::string, std::string> _md5_name;
};