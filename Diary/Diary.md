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

* [03rd](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20181203)

* [05th](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20181205)

* [10th](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20181210)

* [19th](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20181219)

* [23th](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20181223)

***
## 2018/11/20
Project started.

I'm inspired when I find tutorial with Hex map.(https://catlikecoding.com/unity/tutorials/hex-map/)

But in my case, I want to make my own so I'll try with triangular tiles.

And today's work:

![triangular tiles](https://lh3.googleusercontent.com/O1IcRfL1C2NxPlWF6idiOXQxv5JgJ7r7uHm6yzrDXFJ1q7W0nlDUxIZ6GkHXiX_rvSHfN5rYQAJOGoaqEXGvN_IyiMeuFQAwpO8Mbz8JVnUiDFV9EXb7NHgsY1a7yOgZJEvfSZotcPJFLIraAWdMthCeAd1Fxkb7NYD81rjJXStTRLngUO9Y6lovu7or61BaRz-SPAbUxrf-Js934mzbsszeechk5TUJu8MPMFKzuoYCKiDJqEKvLXq8m2-12QgnVLYTWCtvL3kJNjw3e1wFS0KJobaQV2HNjXVpRPtYKJEAPKws72YL2ovfojRccL0J5Q3VBUgwxweiryuV1X-XD56ewax0CM9n1wFgkOCVClNXzxCFI0Lfad6CqHQ0t9jKP_SdQZqI05Oda6kLm-UKcm3eOFS0uAkij-J1wwZz6xvyXu25aXpVafYuD42g2ZKpx0S-XYdNzpYAODcfC7PfdSBr1fteguzNgunahCj5TnlwCOnqnz-RfAyKpwQwiVmEMv3Oud8gpgj4y3zFbbKS1J14JH3CoBX0aRNTK20ivEtzlLqM0pA735HgP1Ri1TgUETjQ_rktlJH51A8mxu3CBdWy-YEnr_9ujyA22JQ7GRJCHJkJ2VqSOVTQcrVWHEhKDMwDJW4evoxudd30x6tzmFU=w1175-h925-no)
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
## 2018/12/05
Finally I established what is 'building','unit', and 'natural object'.

and changed save structure as

application

	isleland name

		buildings.dat

		maps.dat

		naturals.dat

		units.dat

for further modifing.

For now, I can save terrains, units, naturals, and even units' action status!(Yes, Action in queue is also supported and saved!)

And successed in planting trees in whole isleland.

![Do I need more dense mesh?](/Diary/Image/20181205_0.PNG)
***
## 2018/12/10
Until today, I did minor fixing/tweaking, and hold camera into world.

And Today, I made UI for individual building. (only UI,not working at now).

Thanks to Screen To Gif tool, now I'll leave more Dynamic pictures.

![Reaction of building UI](/Diary/Image/20181210_0.gif)

Plus one more!

I felt too bored at studing univ lecture for final exam, so I advanced little more camera/UI things.

Now I can fade in/out any time and make menu for individual building settings(but only object, not script).

![Building Option and fading](/Diary/Image/20181210_1.gif)
***
## 2018/12/19
Finally! Final exam is almost finished.

I'm really, really confused about building up UI structure, so I'm trying many ways.

Until now, I can order units and select units by selecting list in building, but many things can be changed and somethings can be reverted.

For better stuff I built this sutff.

![Does this remains at final build?](/Diary/Image/20181219_0.gif)
***
## 2018/12/23
I made Sun for day/night cycle. (simplified one, can be changed)

And I gave up worldspace Entity Menu because it wasn't great way to show what the entity can do.

Instead of it, I added screen space entity menu with simplified entity information.

I have to add more detailed menu which can show information that not be seen in simplified one.

For now, order command is fine, but have issue in detailed view.

![Time scaled to 10x and screen space located entity menu.](/Diary/Image/20181223_0.gif)

... My desktop's mouse wheel is broken.

So zoom input method added with page up/down key.