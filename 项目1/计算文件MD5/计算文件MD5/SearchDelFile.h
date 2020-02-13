#pragma once
#include <string>
#include <vector>
#include <iostream>
#include <io.h>
#include <unordered_set>

void searchFile(const std::string& path, std::unordered_set<std::string>& files);
void delFile(const char* fileName);