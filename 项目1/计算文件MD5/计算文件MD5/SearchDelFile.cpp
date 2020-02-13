#include "SearchDelFile.h"


void searchFile(const std::string& path, std::unordered_set<std::string>& files){
	std::string  s = path + "\\" + "*.*";
	_finddata_t filedata;
	long handle = _findfirst(s.c_str(), &filedata);
	//找到,返回 0;
	if (handle == -1){
		//没找到
		std::cout << s << std::endl;
		perror("findfirst error :");
		_findclose(handle);
		return;
	}
	do {
		//当前为目录,继续搜索
		if (filedata.attrib & _A_SUBDIR){
			if (strcmp(filedata.name, ".") != 0 && strcmp(filedata.name, "..") != 0)
			searchFile(path + "\\" + filedata.name, files);
		}
		else{
			files.insert(path +"\\" + filedata.name);
		}
	} while (_findnext(handle, &filedata) == 0);
	_findclose(handle);
}

void delFile(const char* fileName){
	if (remove(fileName) == 0){
		std::cout << "delete file success" << std::endl;
	}
	else{
		perror("delete file error :");
	}
}