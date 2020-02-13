#pragma once
#include <string>
#include <vector>
#include <iostream>
#include <io.h>


void search(const std::string& path, std::vector<std::string>& files);
void delFile(const char* fileName){
	if (remove(fileName) == 0){
		std::cout << "delete file success" << std::endl;
	}
	else{
		perror("delete file error :");
	}
}