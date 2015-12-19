* [README.en.md](README.en.md) is the English version of this document.*  
* [README.en.md](README.en.md) 是该文档的英语版本。*  

# 驱动镜

本项目是一个使用 LGPL 开源协议的综合性磁盘工具，用于取代现有的磁盘检测、测试、跑分程序。  
尽管它现在还在开发阶段，功能还不完全，但我们相信经过大家的努力它能够变为最强大的磁盘综合工具。  

## 项目清单

### UI
主界面。基于WPF。  

### BenchmarkLibrary
包含测试逻辑。 

### DetectionLibrary
包含磁盘检测逻辑。  

## 许可 
本项目采用 GNU LGPL 许可。详见 LICENSE 文件。  

## 参与项目建设

欢迎参与项目。  

1. 提出 Issue。  
   如果你  
   - 发现项目存在 BUG  
   - 有好的想法  
   - 需要帮助信息  
   你都可以提出 Issue 以帮助我们做得更好。  
   我们非常感激你的每一分贡献。  
  
1. 贡献代码。
   你可以 Fork 该版本库，针对某个功能或 Issue 编写代码，并提交 Pull Request。   

## 项目历史
本程序的作者之一 @t123yh 有一天无聊，反编译了 `AS SSD Benchmark`，遂开始了此项目前身的制作。  

后来首先制作了一个版本，但是代码质量很差，并且没有使用源代码管理。经过另一位作者（@peng1999）的提醒遂放弃了所有的代码，开始进行此项目。  

## 鸣谢
- [AS SSD Benchmark](http://www.alex-is.de/ "Alex Intelligent Software")  
   提供了项目的思路和磁盘检测部分的 WMI 语句。  

- [CrystalDiskInfo](http://crystalmark.info/ "Crystal Dew World")  
   提供了 S.M.A.R.T，温度以及设备健康状态检测的思路。  
