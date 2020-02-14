#include "FileManager.h"
#include "SearchDelFile.h"


//扫盘
void FileManager::scannerFile(const std::string& path){
	//扫盘之前把files清空
	files.clear();

	//把所有文件名存储到files中
	searchFile(path, files);

	//得到所有文件的file --- md5
	getMd5_Files();

	//得到相同文件的MD5 -- files
	sameFilesList();
}

//将所有文件按存储到两种映射方法中
void FileManager::getMd5_Files(){
	//清空原来的MD5_FILE
	file_md5.clear();
	md5_file.clear();
	for (const auto& e : files){
		//计算每个文件md5之前要重置ABCD
		md.reset();
		md5_file.insert(make_pair(md.getFileMd5(e.c_str()), e));
		file_md5.insert(make_pair(e, md.getFileMd5(e.c_str())));
	}

}

//把重复的文件存储到映射关系为md5 -- file中
void FileManager::sameFilesList(){
	auto it = md5_file.begin();
	while (it != md5_file.end()){
		if (md5_file.count(it->first) == 1){
			it = md5_file.erase(it);
		}
		else{
			++it;
		}
	}
}
//保证相同文件只留下一个,留下当前的文件
void FileManager::delFileByName(const std::string& fileName){
	//文件不存在
	if (file_md5.count(fileName) == 0){
		perror("文件不存在 :");
		return;
	}
	//获取文件md5
	std::string md5 = file_md5[fileName];
	//有多个相同文件存在
	if (md5_file.count(md5) > 1){
		int count = 0;
		auto pair = md5_file.equal_range(md5);
		auto begin = pair.first;
		//删除同内容文件,注意不删除传入的文件名所对应的文件
		while (begin != pair.second){
			if (begin->second != fileName){
				//删除磁盘里的文件
				RemoveFile(begin->second.c_str());
				file_md5.erase(begin->second);
				files.erase(begin->second);
				begin = md5_file.erase(begin);
				++count;
			}
			else{
				++begin;
			}
		}
		std::cout << "按文件名删除成功 :" << count << "个" << std::endl;
	}
	else{
		std::cout << fileName << " : 只有一个" << std::endl;
	}
}

//保证相同的文件只留下一个
void FileManager::delFileByMd5(const std::string& fileMd5){
	//如果fileMD5对应的文件存在
	//md5_file 只存放重复文件
	int count = 0;
	if (md5_file.count(fileMd5) > 1){
		auto pair = md5_file.equal_range(fileMd5);
		auto begin = pair.first;
		++begin;
		//从第二个开始删除,files和file_md5
		while (begin != pair.second){
			//删除磁盘里的文件
			RemoveFile(begin->second.c_str());
			files.erase(begin->second);
			file_md5.erase(begin->second);
			++count;
			++begin;
		}

		auto it = md5_file.find(fileMd5);
		++it;
		while (it != md5_file.end() && it->first == fileMd5){
			it = md5_file.erase(it);
		}
		std::cout << "按MD5删除成功 :" << count << "个" << std::endl;
	}
}

//删除重复的文件,重复的文件都在MD5_file存放
void FileManager::delFileBySame(){
	if (md5_file.size() == 0){
		std::cout << "没有重复的文件" << std::endl;
		return;
	}
	else{
		int count = 0;
		std::string md5;
		//拿到首元素的md5
		auto it = md5_file.begin();
		md5 = it->first;
		++it;
		while (it != md5_file.end()){
			
			if (it->first == md5){
				//删除磁盘里的文件
				RemoveFile(it->second.c_str());
				files.erase(it->second);
				file_md5.erase(it->second);
				//删除MD5_file中的,并进行迭代器更新
				it = md5_file.erase(it);
				++count;//删除文件的个数
			}
			else{
				md5 = it->first;
				++it;
			}
		}
		std::cout << "重复文件删除 :" << count << "个 " << std::endl;
	}
	
}
//模糊删除
void FileManager::delFileByMatch(const std::string& Match){
	//str.find(str) != npos;
}

void FileManager::RemoveFile(const char* name){
	delFile(name);
}

void FileManager::showAllFiles(){
	std::cout << "所有文件 : " << std::endl;
	for (const auto& e : files){
		std::cout << e << std::endl;
	}
	std::cout << "文件总数 :" << files.size() << std::endl;
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
	std::cout << "重复文件总数: " <<  md5_file.size() << std::endl;
}
void FileManager::showMd5Files(){
	for (const auto& e : md5_file){
		std::cout << e.first << "---" << e.second << std::endl;
	}
	std::cout << "文件总数: " << md5_file.size() << std::endl;
}

void FileManager::showMD5_File(){
	std::cout << "\tmd5" << "\t---" << "Filename" << std::endl;
	for (const auto& e : file_md5){
		std::cout << e.second << e.first << std::endl;
	}
	std::cout << "文件总数: " << md5_file.size() << std::endl;
}
void FileManager::showFile_MD5(){
	std::cout << "\tFilename" << "\t---" << "md5" << std::endl;
	for (const auto& e : file_md5){
		std::cout << e.first << e.second << std::endl;
	}
	std::cout << "文件总数: " << md5_file.size() << std::endl;
}