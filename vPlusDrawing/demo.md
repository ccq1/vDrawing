



删除选中的内容。 获取鼠标点击处的坐标。

然后便利Children 集合中的每一个 drawing 实例。（可以是继承自drawing的），然后便利他们的坐标，是否重合。重合即去除。



![image-20230831155612580](C:/Users/79907/AppData/Roaming/Typora/typora-user-images/image-20230831155612580.png)



![image-20230831155704537](C:/Users/79907/AppData/Roaming/Typora/typora-user-images/image-20230831155704537.png)





线 ![image-20230831160726462](C:/Users/79907/AppData/Roaming/Typora/typora-user-images/image-20230831160726462.png)



![image-20230831161002960](C:/Users/79907/AppData/Roaming/Typora/typora-user-images/image-20230831161002960.png)

![image-20230901105758102](C:/Users/79907/AppData/Roaming/Typora/typora-user-images/image-20230901105758102.png)

![image-20230901105946826](C:/Users/79907/AppData/Roaming/Typora/typora-user-images/image-20230901105946826.png)

![image-20230901110245646](C:/Users/79907/AppData/Roaming/Typora/typora-user-images/image-20230901110245646.png)

![image-20230901110255621](C:/Users/79907/AppData/Roaming/Typora/typora-user-images/image-20230901110255621.png)



判断以下，isHasChildren?  false  就是 圆，true就是线条。  二者判断逻辑不一样，圆与鼠标点阵相交的情况。 线与鼠标点阵相交的情况。

如果是圆的情况下。我鼠标点击之后，会产生一个10*10像素的矩形点阵。判断矩形点阵是否与圆相交。采用什么方法？？？ 圆貌似也是一个矩形。









撤销，重做///// Undo  Redo

(maybe 做一个 Redo栈。用于执行Redo命令。 Undo一次，向RedoStack加入Undo的操作，比如Undo是Draw，那么Redo执行Delete命令，并且向命令栈加入一个一个Delete)

创建一个命令栈（Draw，Delete）  draw1  draw2  draw3  draw4  draw5  delete3  delete4   

执行 Undo  -> CommandStack<Command>. Pop() - >  delete4.  if(Command == delete) ->  Opreation = Draw(Gemeotry = delete4.Geometry)  CommandStack<Command>.Push(draw4)

目前图像有 ，1，2，4，5

命令栈如下：  draw1  draw2  draw3  draw4  draw5  delete3  

