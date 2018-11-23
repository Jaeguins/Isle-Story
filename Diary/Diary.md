# Development Diary
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