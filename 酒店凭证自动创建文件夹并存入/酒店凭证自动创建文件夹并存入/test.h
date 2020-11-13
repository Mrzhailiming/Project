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
	myWork(string path = NULL){;
	}

	//获取文件夹下所有文件
	void getAllFile(){
		_finddata_t findData;
		long fHandle;
		fHandle = _findfirst("D:\\test\\*.*", &findData);
		if (fHandle != -1){
			do {
				if (findData.name == "." || findData.name == ".."){
					continue;
				}
				
				pfileVec.push_back(findData.name);
				
			} while (!_findnext(fHandle, &findData));
		}
	}
	//以序号-文件名映射， multimap
	void num_fimeName(){
		//遍历vector, 把序号和文件名映射
		int num = 0;
		for (const auto& e : pfileVec){
			int i = 0;
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

			}
		}
	}

	//创建对应公司/人的文件夹（以序号代替）
	void createNewDir(string& dirPath_name){
		string sh = "md ";
		string tmp = sh + dirPath_name;
		system(tmp.c_str());
	}
	//移动文件
	void moveFile(string& oldPath, string& newPath){
			MoveFile((LPCWSTR)oldPath.c_str(), (LPCWSTR)newPath.c_str());
	}
private:
	string path;
	vector<string> pfileVec;
	multimap<int, string> pMulMap;
};



#endif