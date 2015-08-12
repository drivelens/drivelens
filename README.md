# DiskMagic

本项目是一个使用 LGPL 开源协议的综合性磁盘工具，用于取代现有的磁盘检测、测试、跑分程序。  
This project is a generic open-source disk detection/benchmark program under the GNU LGPL license. It is designed to replace other disk tools.  
尽管它现在还在开发阶段，功能还不完全，但我们相信经过大家的努力它能够变为最强大的磁盘综合工具。  
Although it is in development now, and many of the features are not implemented, but we believe that with the contributions of you, it will become the best disk benchmark tools.  

## 项目清单 (Project List)

### UI
主界面。使用WPF。    
This is the main user interface. It is based on the WPF.

### BenchmarkLibrary
测试逻辑。
This is the module that contains the progress of benchmarking.  

### DetectionLibrary
磁盘检测逻辑。
This is the module that contains the progress of detecting the information of the disks and partitions.  

## 许可 (License)
本项目采用 GNU LGPL 许可。详见 LICENSE 文件。   
This project is under the GNU Lesser General Public License. See LICENSE for more information.

## 参与项目建设 (Contributing)

欢迎参与项目。  
Contributing to the project is strongly welcomed.  

1. 提出 Issue。  
I. Giving issues.  
   如果你  
   If you  
   - 发现项目存在 BUG
   - find any bugs  
   - 有好的想法
   - have any good ideas  
   - 需要帮助信息 
   - need help  
   你都可以提出 Issue 以帮助我们做得更好。  
   You can give issues anytime in order to help us to do better.
  
2. 贡献代码。  
II. Writing code.
   你可以 Fork 该版本库，针对某个功能或 Issue 编写代码，并提交 Pull Request。  
   You can fork this repo and write code for any features or issues, and create a Pull Request.

## 项目历史 (Project History)
本程序的作者之一 @t123yh 有一天无聊，反编译了 `AS SSD Benchmark`，遂开始了此项目前身的制作。  
One day, when @t123yh, an author of this project, was boring, he decompiled the `AS SSD Benchmark`, then the idea poped up to start the predecessor of the program.  
后来首先制作了一个版本，但是代码质量很差，并且没有使用源代码管理。经过另一位作者的提醒遂放弃了所有的代码，开始进行此项目。
Later, he made the first version, but the code has a poor quality and he didn't use the source control. By the remind of another author ( @peng1999 ), he dropped all the code, and then started this project.

## 鸣谢列表 (Thanks List)
- [AS SSD Benchmark](http://www.alex-is.de/ "Alex Intelligent Software")
   提供了项目的思路和磁盘检测部分的 WMI 语句。
   For the idea and the drive detection WMI queries.  

- [CrystalDiskInfo](http://crystalmark.info/ "Crystal Dew World")
   提供了 S.M.A.R.T，温度以及设备健康状态检测。
   For the drive S.M.A.R.T, temperature and device health detection.
