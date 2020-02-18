#include "FileManager.h"
#include <io.h>

void FileManager::init(){
	std::cout << "请输入路径 :";
	std::string path;
	getline(std::cin, path);
	//进行扫盘
	scannerFile(path);
	//计算每一个文件的md5
	getAllFilesMd5();
	//对MD5_file容器进行更改,把不重复的文件删除掉
	getSameFiles();
}

//扫盘
void FileManager::scannerFile(const std::string& path){
	//要匹配的文件格式
	const std::string tar = path + "\\" + "*.*";
	_finddata_t fileData;
	long handle = _findfirst(tar.c_str(), &fileData);
	if (handle == -1){
		perror("find file Error : ");
		_findclose(handle);
		return;
	}
	do{
		if (fileData.attrib & _A_SUBDIR){
			if (strcmp(fileData.name, ".") != 0 && strcmp(fileData.name, "..") !=  0){
				scannerFile(path + "\\" + fileData.name);
			}
		}
		else{
			_allFiles.insert(path + "\\" + fileData.name);
		}
	} while (_findnext(handle, &fileData) == 0);
	_findclose(handle);
}

//计算每一个文件的md5,并放入对应的容器中
void FileManager::getAllFilesMd5(){
	//清空
	_name_md5.clear();
	_md5_name.clear();
	std::string md5;
	for (const auto& e : _allFiles){
		md5 = _gMd5.getFileMd5(e.c_str()).c_str();
		_name_md5.insert(make_pair(e,md5));
		_md5_name.insert(make_pair(md5, e));
	}
}

void FileManager::getSameFiles(){
	std::string md5;
	auto it = _md5_name.begin();
	while (it != _md5_name.end()){
		md5 = it->first;
		if (_md5_name.count(md5) == 1){
			it = _md5_name.erase(it);
		}
		else{
			++it;
		}
	}
}

//展示
void FileManager::showAllFile(){
	std::cout << "\t文件数 :" << _name_md5.size() << std::endl;
	for (const auto& e : _name_md5){
		std::cout << e.first << "\t--\t" << e.second << std::endl;
	}
}
void FileManager::showSameFile(){
	std::cout << "\t文件数 :" << _md5_name.size() << std::endl;
	for (const auto& e : _md5_name){
		std::cout << e.first << "--" << e.second << std::endl;
	}
}
void FileManager::showName_Md5(){
	std::cout << "\t文件数 :" << _name_md5.size() << std::endl;
	for (const auto& e : _name_md5){
		std::cout << e.first << "\t--\t" << e.second << std::endl;
	}
}
void FileManager::showMd5_Name(){
	std::cout << "\t文件数 :" << _name_md5.size() << std::endl;
	for (const auto& e : _name_md5){
		std::cout << e.second << "\t--\t" << e.first << std::endl;
	}
}

//删除,去重
void FileManager::delFileByName(){
	std::cout << "请输入去重的文件名称 :";
	std::string name;
	std::cin >> name;
	//std::cout << _allFiles.count(name) << std::endl;
	if (_allFiles.count(name) < 1){
		perror("file not found :");
		return;
	}
	//删除的文件个数
	int count = 0;
	std::string md5 = _name_md5[name];
	auto pair = _md5_name.equal_range(md5);
	auto beginIt = pair.first;
	while (beginIt != pair.second){
		if (strcmp(beginIt->second.c_str(), name.c_str()) != 0){
			++count;
			//remove(beginIt->second.c_str());
			_allFiles.erase(beginIt->second);
			_name_md5.erase(beginIt->second);
			beginIt = _md5_name.erase(beginIt);
			//重新获取区间
			pair = _md5_name.equal_range(md5);
			beginIt = pair.first;
			++beginIt;
		}
	}
	std::cout << "\t删除文件数 :" << count << std::endl;
}

//
void FileManager::delFileByMd5(){
	std::cout << "请输入去重的文件的MD5 :";
	std::string md5;
	std::cin >> md5;
	//在存放重复文件的容器中查找
	if (_md5_name.count(md5) < 1){
		perror("file only one :");
		return;
	}
	//删除的文件个数
	int count = 0;
	auto pair = _md5_name.equal_range(md5);
	auto beginIt = pair.first;
	++beginIt;
	while (beginIt != pair.second){
		++count;
		//remove(beginIt->second.c_str());
		_name_md5.erase(beginIt->second);
		_allFiles.erase(beginIt->second);
		_md5_name.erase(beginIt);
		//更新迭代器
		pair = _md5_name.equal_range(md5);
		beginIt = pair.first;
		++beginIt;
	}
	std::cout << "\t删除文件数 :" << count << std::endl;
}
void FileManager::delSameFile(){
	int count = 0;
	std::string curMd5;
	auto it = _md5_name.begin();
	while (it != _md5_name.end()){
		curMd5 = it->first;
		++it;
		while(it != _md5_name.end() && strcmp(it->first.c_str(), curMd5.c_str()) == 0){
			++count;
			//remove(beginIt->second.c_str());
			_name_md5.erase(it->second);
			_allFiles.erase(it->second);
			//更新迭代器
			it = _md5_name.erase(it);
		}
	}
	std::cout << "\t删除文件数 :" << count << std::endl;
}

void FileManager::getFILEmd5(){
	std::cout << "请输入文件名称(包含路径) :";
	std::string name, str;
	std::cin >> name;
	str = _gMd5.getFileMd5(name);
	std::cout << "文件的md5为 : " << str << std::endl;
	FILE* f = fopen("Filemd5.txt", "a");
	fwrite((name + " <-> " + str + " --- ").c_str(), 1, name.size() + str.size() + 10, f);
	fclose(f);
}
void FileManager::getSTRINGmd5(){
	std::cout << "请输入字符串 :";
	std::string name, str;
	std::cin >> name;
	str = _gMd5.getStrMd5(name);
	std::cout <<"字符串的md5为 : " << str << std::endl;
	FILE* f = fopen("Strmd5.txt", "a");
	fwrite((name + " <-> " + str + " --- ").c_str(), 1, name.size() + str.size() + 10, f);
	fclose(f);
}