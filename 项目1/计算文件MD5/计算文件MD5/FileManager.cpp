#include "FileManager.h"
#include "SearchDelFile.h"


//扫盘
void FileManager::scannerFile(const std::string& path){
	//扫盘之前把files清空
	files.clear();

	std::cout << "All files" << std::endl;
	searchFile(path, files);
	showAllFiles();

	std::cout << "MD5 -- fileName" << std::endl;
	getMd5_Files();
	showMd5Files();

	std::cout << "samelist" << std::endl;
	sameFilesList();
	showSameFilesList();
}

//将文件按md5 -- file 映射
void FileManager::getMd5_Files(){
	//清空原来的MD5_FILE
	md5_file.clear();
	for (const auto& e : files){
		//计算每个文件md5之前要重置ABCD
		md.reset();
		md5_file.insert(make_pair(md.getFileMd5(e.c_str()), e));
	}
}

//只保留重复文件,映射关系为md5 -- file
void FileManager::sameFilesList(){
	file_md5.clear();
	auto it = md5_file.begin();
	while (it != md5_file.end()){
		//如果有两个以上的文件MD5值相同,则放入
		if (md5_file.count(it->first) > 1){
			auto pair = md5_file.equal_range(it->first);
			auto begin = pair.first;
			while (begin != pair.second){
				file_md5.insert(make_pair(begin->second, begin->first));
				++begin;
			}
			it = pair.second;
		}
		else{
			files.erase(it->second);
			it = md5_file.erase(it);
		}
	}
}
//保证只留相同文件只留下一个
void FileManager::delFileByName(const std::string& fileName){
	

}
void FileManager::delFileByMd5(const std::string& fileMd5){

}
void FileManager::delFileBySame(){

}
//模糊删除
void FileManager::delFileByMatch(const std::string& Match){

}

void FileManager::showAllFiles(){
	for (const auto& e : files){
		std::cout << e << std::endl;
	}
}
void FileManager::showSameFilesList(){
	auto it = md5_file.begin();
	while (it != md5_file.end()){
		auto pair = md5_file.equal_range(it->first);
		auto begin = pair.first;
		while (begin != pair.second){
			std::cout << begin->first << "---" << begin->second << std::endl;
			++begin;
		}
		it = pair.second;
	}
	std::cout << md5_file.size() << std::endl;
}
void FileManager::showMd5Files(){
	for (const auto& e : md5_file){
		std::cout << e.first << "---" << e.second << std::endl;
	}
}