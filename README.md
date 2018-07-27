# 消毒供应中心(CSSD)登记系统

消毒供应中心(CSSD)登记系统是我在福建中医药大学附属第三人民医院信息科见习的时候写的小系统，目的是解决消毒供应中心(CSSD)长期以来人工统计各科室使用器械/包效率不高且过程繁琐的问题，期间经历一次CSSD大姐姐的重大需求变更，差点全盘重构，现在已经愉快的上线使用啦！

软件版本：v1.0.0

编写日期：2018年7月27日


## 一、简介

医院消毒供应中心登记系统是一套适用于消毒供应中心（CSSD）对器械/包进行登记管理的软件系统。通过系统记录即可知道全院科室的器械/包的使用情况。

## 二、系统结构

系统各功能模块组成结构图如下：

![](https://github.com/jl223vy/fjtcmfs-cssdms/raw/master/Img/StructureDiagram.jpg)

各功能模块的具体功能如下：

- 器械/包管理：
  - 新增记录
  - 删除记录
  - 修改记录
- 导出Excel
- 清除数据


## 三、使用说明

### （一）系统安装

1、运行环境要求

- Windows XP/Windows 7或以上操作系统
- Microsoft office 2003 或以上版本
- Microsoft .NET Framework 4.0 或以上

2、安装

- 软件无需安装，在使用之前发送“消毒供应中心登记系统.exe”的快捷方式至桌面即可。
- 数据库“CSSDBase.mdb”和“消毒供应中心登记系统.exe”必须放置在同一路径中。
- 若目标电脑缺少Office，将影响“导出Excel”功能，其他功能不受影响。
- 在正式使用本系统之前，请执行【清除数据】功能，密码为：123456。

### （二）系统使用

**1、新增记录**
![](https://github.com/jl223vy/fjtcmfs-cssdms/raw/master/Img/add1.jpg)
![](https://github.com/jl223vy/fjtcmfs-cssdms/raw/master/Img/add2.jpg)

- 功能说明：通过在右侧“记录信息”栏输入记录信息，新增一条记录。
- 操作说明：
  - 点击【新增】按钮，在右侧“记录信息”栏中填写记录信息，再点击【保存】完成新增一条记录。
  - 【包/器械选择框】中输入包/器械的首字即可完成自动填充，选择的包/器械会自动加入至下方文本框中，文本框中的内容可自由编辑。

**2、删除记录**
![](https://github.com/jl223vy/fjtcmfs-cssdms/raw/master/Img/del.jpg)

- 功能说明：删除选中的一条记录信息。
- 操作说明：
  - 选中需要删除的记录，点击【删除】按钮，完成删除当前选中记录操作。

**3、修改记录**
![](https://github.com/jl223vy/fjtcmfs-cssdms/raw/master/Img/modi.jpg)

- 功能说明：通过在右侧“记录信息”栏修改记录信息，修改一条记录。
- 操作说明：
  - 点击【修改】按钮，在右侧“记录信息”栏中修改记录信息，再点击【保存】，完成修改当前记录。
  - 【包/器械选择框】中输入包/器械的首字即可完成自动填充，选择的包/器械会自动加入至下方文本框中，文本框中的内容可自由编辑。

**4、清除数据**
![](https://github.com/jl223vy/fjtcmfs-cssdms/raw/master/Img/clear1.jpg)
![](https://github.com/jl223vy/fjtcmfs-cssdms/raw/master/Img/clear2.jpg)

- 功能说明：清除数据库中的所有记录信息。该操作需要输入密码以确认操作，密码为：123456。
- 操作说明：
  - 点击【清除数据】按钮，输入密码，完成清除数据操作。
  - 清除数据后，记录号将从1重新开始编号，请慎重进行此操作。

**5、导出Excel**
![](https://github.com/jl223vy/fjtcmfs-cssdms/raw/master/Img/export.jpg)

- 功能说明：导出指定时间区间内和排列方式的记录至Excel表格。
- 操作说明：
  - 修改右侧“导出规则”栏中的开始/结束时间、排列方式，左侧表格中显示的记录将发生相应改变，点击【导出Excel】按钮，即可导出左侧的表格，所见即所得。
  - 表格默认显示最近三天的记录，排列方式默认为“按时间排列”。

 
## 四、下载

[Download CSSDMS v1.0.0](https://github.com/jl223vy/fjtcmfs-cssdms/raw/master/CSSDMS-v1.0.0.zip)


## 五、最后

希望消毒供应中心的大姐姐不要再变更需求了，感恩！

