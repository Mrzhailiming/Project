#ifndef FIRST
#define FIRST

//头文件
#include <iostream>
#include <vector>
#include <string>
#include <vector>
#include <map>

#include <windows.h>
#include <stdio.h>
#include <io.h>
using namespace std;

#define DEFAULT_PATH "D:\\"


class myWork{
public:
	myWork(string pt = NULL){
		path = pt;
	}

	//获取文件夹下所有文件
	void getAllFile(const string& path){
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
				if (strcmp(fileData.name, ".") != 0 && strcmp(fileData.name, "..") != 0){
					getAllFile(path + "\\" + fileData.name);
				}
			}
			else{
				pfileVec.push_back(path + "\\" + fileData.name);
			}
		} while (_findnext(handle, &fileData) == 0);
		_findclose(handle);
	}
	//以序号-文件名映射， multimap
	void num_fimeName(){
		//遍历vector, 把序号和文件名映射
		
		for (const auto& e : pfileVec){
			int i = 0;
			int num = 0;
			while (i < e.size() && (e[i] < '0' || e[i] > '9')){
				++i;
			}
			while (i < e.size() && (e[i] >= '0' && e[i] <= '9')){
				num = num * 10 + e[i] - '0';
				++i;
			}
			if (num != 0){
				pMulMap.insert(make_pair(num, path + e));
			}
			
		}
	}

	//根据multimap 创建文件夹并放入文件
	void create_moveFile(){
		multimap<int, string>::iterator it = pMulMap.begin();
		while (it != pMulMap.end()){
			//遍历
			int num = it->first;
			string dirName = num + "";
			string dirPath_Name = DEFAULT_PATH + dirName;
			//创建新的文件夹
			createNewDir(dirPath_Name);
			//放入文件
			string oldPath = path + dirName;
			string newPath = dirPath_Name;
			moveFile(oldPath, newPath);
			++it;
			if (it->first == num){
				oldPath = path + it->second;
				moveFile(oldPath, newPath);
				++it;
			}
		}
	}

	//创建对应公司/人的文件夹（以序号代替）
	void createNewDir(const string& dirPath_name){
		string sh = "md ";
		string tmp = sh + dirPath_name;
		system(tmp.c_str());
	}
	//移动文件
	void moveFile(const string& oldPath, const string& newPath){
			MoveFile((LPCWSTR)oldPath.c_str(), (LPCWSTR)newPath.c_str());
	}
private:
	string path;
	vector<string> pfileVec;
	multimap<int, string> pMulMap;
};



#endif