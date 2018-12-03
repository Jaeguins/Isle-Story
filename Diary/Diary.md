# Development Diary

## Index

### 2018 November

* [20th](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20181120)

* [21st](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20181121)

* [22nd](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20181122)

* [23rd](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20181123)

* [26th](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20181126)

* [27th](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20181127)

* [28th](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20181128)

### 2018 December

* ![03rd](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20181203)

***
## 2018/11/20
Project started.

I'm inspired when I find tutorial with Hex map.(https://catlikecoding.com/unity/tutorials/hex-map/)

But in my case, I want to make my own so I'll try with triangular tiles.

And today's work:

![triangular tiles](/Diary/Image/20181120_0.PNG)
***
## 2018/11/21
Blending colors with near tiles finished.

I'm nearly get tired of change hexagonal tile in tutorial found yesterday to triangular one, but finally I did it.

And it makes me skip filling between blendered tiles(because it doesn't have any gap!).

So today's work:

![Blendered tiles](/Diary/Image/20181121_0.PNG)

Colors were put for visibility.

I realized that I must not skip bridging-filling between blendered tiles, so turned back to that.

And reform Tiles' coordinate calculation, value of enums, etc..

Finally I got this:

![Wired tiles](/Diary/Image/20181121_1.PNG)
***
## 2018/11/22
Throwed away blending but get elevation.

I don't need blending because triangular tile is only the part of hexagonal tile.

And they have to be seperated surely.

So I removed blending(actually space for blending) and use it for filling filler for elevation difference.

In addition to that, I added noise in shape of tiles.

It makes tiles more naturally.

today's temporal work:

![Triangular tiles](/Diary/Image/20181122_0.png)

Additional Work!

I updated UI for testing and generate example randomly hexagonal tiles.

But still editable with Triangular too.

![Hexa-triangular tiles](/Diary/Image/20181122_1.PNG)

***
## 2018/11/23
Decide to go my own load.

In river generating, hexagonal tile and triangular tile have lots of differents.

That makes me to have train about more mathmetical thinking about 3d coordinates and polygons.

I worked all night(in this case, from yesterday), and finished river generating polygons.

![river!](/Diary/Image/20181123_0.PNG)

***
## 2018/11/26
After full rest in saturday/sunday,

I made water(now in color blue, elevation 0).

today's work:

![blue means water](/Diary/Image/20181126_0.PNG)
***
## 2018/11/27
Isleland Generating added.

....Only in Tirangular.

I need to calculate this with hexagonal one.

But it's beautiful for seing to me.

And I gave up texture. Instead, I added Y-axis perturb for dynamic visibility.

today's work:

![river not included in generating](/Diary/Image/20181127_0.PNG)
***
## 2018/11/28
Now I can calculate hexagonal tile!

And rivers too!

Now I can out from terrain-stuffs..

![river included in generating](/Diary/Image/20181128_0.PNG)
***
## 2018/12/03
First note in december.

Now I can pathfinding and moving units.

And finished get all what I want in tutorial(again, https://catlikecoding.com/unity/tutorials/hex-map/).

But because of following tutorial, I need to clean up the whole project.

(unnecessary methods, class extending relations, etc...)

So, perhaps I'll remains notes less often than before.

What I finished in today:

![while moving in routes](/Diary/Image/20181203_0.PNG)
***