#pragma once
#include <string>
#include <unordered_map>
#include <unordered_set>
#include "MD5.h"
class FileManager{
public:
	void scannerFile(const std::string& path);
	void getMd5_Files();
	void sameFilesList();

	void showAllFiles();
	void showSameFilesList();
	void showMd5Files();
	void showMD5_File();
	void showFile_MD5();
	//删除操作:保证只留相同文件只留下一个
	void delFileByName(const std::string& fileName);
	void delFileByMd5(const std::string& fileMd5);
	void delFileBySame();
	//模糊删除
	void delFileByMatch(const std::string& Match);
	//真正删除磁盘里的文件
	void RemoveFile(const char* name);
private:
	//保存所有文件
	std::unordered_set<std::string> files;
	//md-file,
	std::unordered_multimap<std::string, std::string> md5_file;
	//file-md
	std::unordered_map<std::string, std::string> file_md5;
	md5 md;
};