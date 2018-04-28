# NaiveRC
C#编写的WPF歌词显示控件，支持逐字定位描色。以及一个简单的NRC描色歌词制作工具。但是所有项目均未完善，仅是雏形，而且代码非常垃圾。

### 目录说明
 - NaiveRC.NRCTool 歌词制作工具
 - NaiveRC.Sample  歌词显示控件调用示例
 - NaiveRC         歌词显示控件

### 歌词控件调用
1.加载歌词：`控件.LoadNRC(歌词字符串);//歌词字符串可以是网易云的也可以是NRC的`

2.在播放音乐开始时实时调用此方法更新歌词：`控件.UpdatePositionTime(播放进度总毫秒);`

3.调整音乐进度后要调用此方法重新定位歌词：`控件.ResetPositionTime(播放进度总毫秒);`

4.暂停音乐时调用此方法（同时不要再UpdatePositionTime）：`控件.Pause();`

### 演示图片
![](https://github.com/NaiveNET/NaiveRC/blob/master/NaiveRC.Sample/%E6%92%AD%E6%94%BE%E6%BC%94%E7%A4%BA.gif?raw=true)
![](https://github.com/NaiveNET/NaiveRC/blob/master/NaiveRC.Sample/%E6%AD%8C%E8%AF%8D%E5%88%B6%E4%BD%9C.gif?raw=true)
