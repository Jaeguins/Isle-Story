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

### 2019 January

* [05th](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20190105)

* [06th](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20190106)

* [16th](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20190116)

* [23th](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20190123)

* [29th](https://github.com/Jaeguins/Isle-Story/blob/master/Diary/Diary.md#20190129)

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

![Blendered tiles](https://lh3.googleusercontent.com/bgbQAYNeJteN5tt36ZVO_txpVOFoAOMg0wwrrR6YAO3y0S_AEh4yUEFvRCmoqVBi_6TFRZp90moLooDnQeU7YppyHK3FwYundjLbxNTJum_iplt_VM8bAEiV5YxzjcC0xvSZxgJb-kLj7kw9_KpWBedWlp1RFHM62jQk35YpfjZsO8_yWTwH5yKrjHuwy4jbmzSSbG_r5GHnNiINGfrfiDFu0ibR-S8T8knG3beLkfEJjGCa_X2mEEn8wn-ZgBPVt_vRdWhKSqBHrsUIq5yVVupuoSIFtT7m0cvBQSnD9TIUuHIUmSFa9XC0kCnVqW-c_hsP4N-A6IwmOyQqsBFmVsHA3oluXwksqecrF0SDcyam_-asA3c8skgd4CUIYtOP0h5k8kGO4edprNXdicqvyr_Z2zaowAISgBsOrcaqvOcDemxo4K3SVF-w9rhf7fjC1tL0Bj1H4P7gyIR6PdJi--5mmZ00-ClBsbSsqtzPhCdWvB_rqCss0e9kyuq7Fj1B24GLKv_DqkdKWgt5J2OmjbPmFyX7CkJD2Mp1jbKpH98Jd_JMFD_LMyvoRQdz8dGBzv-ZqPZsMn_iemWOSuOWKpJ1pCn0vMU9jzHn8IcPbImeagZISdGhnFWEvLSD98rTUSYELGF2XoKiVB2oh-3yjGk=w640-h953-no)

Colors were put for visibility.

I realized that I must not skip bridging-filling between blendered tiles, so turned back to that.

And reform Tiles' coordinate calculation, value of enums, etc..

Finally I got this:

![Wired tiles](https://lh3.googleusercontent.com/Zy6a8j1OR8x6TkLvIWK1YPddxnC0G5hn4-nxlQZo_6XH1D5YkUWxIeXcxpGsY7giWwNvI7IXoHqRKzMcA1OrzdkZHme4t_YkUs1LbKwf1h1Zbe3jDOzeuVNTQ690tUMQkJ6G6A-4UQ2-0_1TEEftPrZ80o29_EMR4bw8h3-zt0kFIMFVWuAMGl1ZvjDCda0Cipe5gKZIQHCv_zmmRRPfweEr7hSMlkSdeHIwzw5odAnQwL6MQpxDUk1Vhq5mBsbliPd8aL5zJOED9cDItXOc34AR2FHjFnh4lp_OJRR8C4W3KtXq1GStZ_TWw-f3TqA6jrTXPhzsHubDR1aKyEwr86LrIfN7MPmXC8QYRxa-R923tLIzNWlKTuD6msHy9u9jzLlt8-dlOxZdEzEKalTsGxi1PfWUi-cZSZRjeGeCgaEMbExE4H6UBTW-e8P7M_6aqLLvuIEtZ1e8mXzWwUlLyzmfR17FNWldTebHjK2V6RF-UkZK24UCi5E2rIVuRTmrltZk3rg8mBJp0IkgyPKvs7v3qVQJj7XQ42_STtGNc8arie3ASJ5N3uaByAdyhd-K3pO--ekMzS8rRnH5k0xk4V0DX32Cpp-fABdS8AjaWeHog0LhFNoQVt6gEukh-NwQDY9Vd57u121tC70sIdwN1rLSXAHn_gp3Bw9JOwth5R72YC7KkexFhI4AKeVtkQQ8pNXDXAoxoVhYw-fp=w561-h471-no)
***
## 2018/11/22
Throwed away blending but get elevation.

I don't need blending because triangular tile is only the part of hexagonal tile.

And they have to be seperated surely.

So I removed blending(actually space for blending) and use it for filling filler for elevation difference.

In addition to that, I added noise in shape of tiles.

It makes tiles more naturally.

today's temporal work:

![Triangular tiles](https://lh3.googleusercontent.com/nG46AenSdpOEPO-1OFEMc_-zRNcRSP92gw3QijTT2-0WTzmi0b1pbR7iNUqOrL4xBAMJUviPXIfjLSH6zlWAmM21OJNIOqf402dPoOohqBK3U_t0he8ht5RSrgRadU2bnvJddw_4PNs9pA34AZcAC2bO7n7PJk8tYA-lNmddV5dSSrNDpdd0aArNCJ_Dt9p-40Lu-x7j0mTqzoxE84lMlBM2gk0qj9WF94ZPFjU9xRiPTCd-6CQZSt3PWlUtMVrkbvMAwMfogvp0hTwpuoQMKv4wcEkWBRXPUqNC3z69a_sKwHOZKtt3hnZXVSk4zJN2lpKNHWJqRCS1sp56-nv0nVAVadKmRQfzijidWPFtEW7MdhOeaPYkY8Vh-zSPoRtUgmXGV6ZozX8rbB3P9tlx-Lxv-D7rWmEIY2qKszJ72VQLVM5ZpoDYMFftICmTry4qjiXKCc3GUzrwY4afSvvy2MxHgBWDw_-EzqmZak24bdq4H5_DIB6saZ2iDkI-m-I5H4qF5ePXeXkoPeWbwRqjD9dYBXiWztQ5cJ4x0dA2eutvpkIJE6EoJrCn-SGHVFd-ikNoZzWj8fcis1jLhd9teUdc571noMZTLX1DIfo9Nueb4wM2qo0zgi1WaiQGQDl2hfR9Uh5nWgnDwl3paE78l4ffglmzdSHDXmvuCrpTSZMASxpVHCRYm38TXR-CQ8V_iyusOKhAvHiC1yDz=w656-h497-no)

Additional Work!

I updated UI for testing and generate example randomly hexagonal tiles.

But still editable with Triangular too.

![Hexa-triangular tiles](https://lh3.googleusercontent.com/lgLNR3rcoRBqNvGwiAjAWjNnpceLSr98VWR-IOi0tuoIz9g1O2Kgf_DqszrsRCzWgG9gMa3SdvgMlyDZzr5JoVF1OZvK-dduH9Ksz0r4ZcJt-doyqaEBntdlr69TYLe4kCP0FPXdds_jx-KupXTJ1hMLv79yXZZm4WopIVBlDj1jytbWiXuMS7NczBEClDFvC_5edRXQpkkiRrIGXnTeg5n3xkdhbW4rstaRXoHTGfDLI8vguTLwiDTUpcUgem2qE0gLcIPmYzoYY-JajAdCGN1ZX2QNpiQxoGKxeenQ7Z6e7EG3_drvnYJ-UgF-drljOGV1w129clv0BnUo3mVQ0siJbaVUbL4JaDiOBCYzqhK7gz4Emtq9jJEIqZLhu_YPV7hJkKuleBzDIQ8IrAd3vJNSUfxTNcNwp9eUznBKTbPnLgNspmFsb8s3tibw5L3__2xcwUxHsKLxrNAAf4T1iKr_LgefNhtU2KEL1qC1iGKlirMfarrFDIsgO8othW6fLnXY4Lx5qz5grCY4Ow7UJTtpTt0N5H4eZyonu5VwNBVU9O9cRrapIFXtPrIL-2iW0IdRRGF-Gd-1VPtkm-Bzx_gICraKbny5lSS61b0g_6xmg0yi0VPgjwXBJfEZ7dxeTrNFusIJdBGfJdl7LTrp1Enqc5nLVgu2DzdJiG3vw_DbcGgQJTlyFIykI93Ixx7nk9KU1KpWPKSj9AsT=w1096-h626-no)

***
## 2018/11/23
Decide to go my own load.

In river generating, hexagonal tile and triangular tile have lots of differents.

That makes me to have train about more mathmetical thinking about 3d coordinates and polygons.

I worked all night(in this case, from yesterday), and finished river generating polygons.

![river!](https://lh3.googleusercontent.com/UxPmxNhzAd-O8fvD13aiQvk3XOzJfiOmc9qMCucHBuVZJkldzbxPAwwMFE7XqJUtKt0anddgJ7jn9kwWTA8VMphhxcUrfJ2SP2WOpsUhamnuKqqkLh7ROh0DDmOSkiS5i5eL2zOR3L6uMfGcG6WNLGI6duxPakxmiOokxZg2RTF7dvw32mK9ddmYSItq5GCkkJ5G-WdBvbHuecLK_L4suFEcAqlkZJt4j0_9p6WQo63nFC4qOjD1th_QWIaT3WNmLeqthzep5Pm6MsADiAHDFJ_gVdTk6FYFSkOAh4WLysXx_JrD38MNa_rzkv7PPZ2JGyhUl0KOPkVXoPvVxCZwz0b-f5E8offF2rad0_IopQyFjS4ynryBJ9NkrYbSo7wZyZr-R55MaB1qvfrcHvMr5bWqkzGIJALfcwkrYVUJZMDOPvtCWozHGUe7xeIUrXdL41TI4la-IFB_-pLQWk0TFDOnZgksKfchyHQqDvY2UdNXeS335sCzNhp8hM_3Aik47U_NFwEr9U-7C7EAkQrZdohk-h7j6SybwxvyW4zBYSEYLoc09OnmZ6F1XMWqG86NIAP4iHzuXOrgHT8niuVabsFPoTI_2-VfpCTSrEEoDDRkS0MosjmwgKl_gsFAyx-maAgVN073r4qLrdIQvuXc5hQWipm6tT0Tpgkgl0h45ahnTplXdKiWLXbySPFQmmv6Ja5Al3xFX0MJbBl4=w775-h518-no)

***
## 2018/11/26
After full rest in saturday/sunday,

I made water(now in color blue, elevation 0).

today's work:

![blue means water](https://lh3.googleusercontent.com/oJ5DZdB_IWDTQvq-taf3qqQTcKkO2OHW_UFHqhU_DLZ7edDbGHb-UvBVpNwCCDCLvt01QIL7Kh9OO29pRJ-vm1bpjqkkdzSXvrpnXenBrqwbKiqity_Eix2QWzncEkFD1agFbevJhnByr2pUQaiXYPvAURfECC1DsarmMBks9CgwSTDyztSQTNgFD6QXMrcgvFWLm48BOaLmWZ4qf1R5lCWTOd4kYGB7EIO_pIws_Cb2IVY8pmnRi7wWOVlngDPvSbS2UPIBt6Ne5vgwmtp_3FZAVIklJk-sb07bOAFJMhf1sdA2WuBRCUaEnRcgswdfo2AX2voMyo5ch3hkKEBc_bkcInKCUm6Q4Pa3eDtF8YO9p3Ki34WDK18GaRr1V9Xf08ES-q8gg5B87Ofxbt_V71LFRHIcSsA1DwhR-fdBlZ124c_bcVO-BI5nMZBH0SH0n8fctRlkgJfw573iePMgnlqWgxG-8Q9EJQFgvnARqk5WWGU4-m-sxrtZSfF0J0UL_jlvrGi_C858rBJjBlrRXoAnyc2sp9LI29EMLIjsXmK2Hmzcc7fnV1WaTAfVIC_0fb3H79zuuSlZiZ8ui02_cbg5vI1QU486h69hFD1R9eXucIoLWpadd60NC5wn-PtjfyeAN297VcLXctxQaHnVo6oiJlTUYiI3MU2pT_lj8yuGstQdOq4_mZ4zXTtY0dqLiBhDPTrYS_cx2J38=w693-h380-no)
***
## 2018/11/27
Isleland Generating added.

....Only in Tirangular.

I need to calculate this with hexagonal one.

But it's beautiful for seing to me.

And I gave up texture. Instead, I added Y-axis perturb for dynamic visibility.

today's work:

![river not included in generating](https://lh3.googleusercontent.com/K57rfC3aYSNWMcGIYFtp9mVt9gwPcRWbAN6AW4_gqjdD-vDqss2kvGbGfAATw7kt3wNVL0DXdrqmUJFRkO6eb_D8BiOGu9qcK5UXOksEJZNYyscvIlSnsvSzy_WBZ_qi4ngW7zxZ1OPAfBSBk8nIj6zB2q1uZy-aPiFHytDXmdRZGNDoSeBGux82qsz-W9c7R5mUNAHExzBco6GN_nnfKQqTcXT2QpYTGt5YDw5aDZVil2c3HM5_ETGhc9OWFjKdehU5YUsCG1rfkXYz7E-3yV_JCnO6gNfZnAQ8PNN_q6m6fh-WA1ukahI51Muzpy_5iW1DIIKKGkvj12IveXPIJE8vhBLQcplTwwN9HmLQuoF3mfeKoMidQnwSAamySj9Cfz2EMiaCP0rxhd6xFwd5nTfu1lx7YMOYDsc8Z11VkIZO1jb5yQbXaGR8a4jLvtBGyx4Ac1CCgxnuIspV9ZHTHcgbjc10Mbz7PJnkoWbNS2UiHNTmpBHzvuCgYNKmvD6SBQROpvYND2qpSSbeWY5kolAwD64WfEqcOSu4AN1ZDUqKglWQFb3LOU1eSsEDg-q-wb_GNya-W6cKGT1cy3eLtBZEGAPXOi4LVw4OsSvacTPUXEuz7mp7DhZeyQjrGpiIhrC-7Ox2KQFbDw_0z8Hb27A=w801-h514-no)
***
## 2018/11/28
Now I can calculate hexagonal tile!

And rivers too!

Now I can out from terrain-stuffs..

![river included in generating](https://lh3.googleusercontent.com/KM9buDQay3Xg_ScMONr5vgxRjRAGrPHLr-vO_8y8DSH0U0vbkPXWBuDv4X8HvMOWu2FKqQSJNIHZAxHYFoEQlKNRgaz7nKmUfc9QZLus2jTOO78MgN0UyVLuocpEG9TpGPeyTk9UYeIVfUd50w4G7_1Ko-wTCSwt3BiH-HbORwDViB8Wl4ybpbJHvrWKigGBcrybNWfjvfmbFRBdyTHRMzGPgkD62142ta2ocXCyBnpzIAfs7vW9KnB_IHBeJ1gbixIddoC9g-22IKtftsIXnrM6fZEVcerl7Du-McptlLjuBRCcJ3waztHHwZk66BWlYhdmLKvMhCFSfjBP1ZsYVlmyWhOPQr2_MfJuvdx4xRQ7W0pcIB3y36b_KXmOTHzQpKqvbPUjlsU5As6Ttn-NFkPvWl3UuN0PPXS-_RR0O7tR8smVwhM8URYUVB-5lsWo5v5g9H_BiYiBP4X1s5FQzAnH6up4ew75_0dGDefFNR-M6G83pvS785lenJxSdbGrfRgElBgi2m3_sF8Bp3LqG3LOJy4SuaqpPoUWd9OEF4QqF26wZaNDRVIZfNN4dtAjgbRpdtBz7RI969F76D3yljvO6K7Pw4Pm-E9Nrjt4FYgtflDENm9rDYP0yS56p1M6ZVsfGIxwEKwdqCEWDECBwMRx78TZW9MbndWF4zS6ibF9OElhr0amk4Djqew4xr3E0GkFnHzBd_fvyvWV=w584-h386-no)
***
## 2018/12/03
First note in december.

Now I can pathfinding and moving units.

And finished get all what I want in tutorial(again, https://catlikecoding.com/unity/tutorials/hex-map/).

But because of following tutorial, I need to clean up the whole project.

(unnecessary methods, class extending relations, etc...)

So, perhaps I'll remains notes less often than before.

What I finished in today:

![while moving in routes](https://lh3.googleusercontent.com/NdjhLum6j5vIAs3DF5xMcoaImuyrFOdyEOf1SX42fm2XXhN-5Qd_v3rnk6C8INfxt-3Csv0iB1PSfEu3yExQHPbwsEMdkzNN5e-aKZowt_tq-yf9dJeFocf4yN3lDndbfF6lETT5w7IopMHPOWtUD-14YRuNI3ssnX6ahx4nZAwJlByGPqI7ekgphafkQI3MPwuMhm5skUDBocyog36ebF14al3P6ngUF_7wVBVYVx8ppq7OuFuSB-Y0hIAlhpXGCTSVX4GmWvpij5pmMeVKVnGolDpmsR7A2st86Z-WsOyTXL9PFaHsmhnWgtYIFswmMqd62ZFBEqXLlISWQG2DWlerCCAqTaqgT3IPBoa2YxEfJUwBmXKtJKiEgdgcrK_BtsuUFuKXNah3qkR7il35l3ljbr9Oj_SzTibpxROXi_jVq3x_eKScaF6KzS3_-LufTjjBJp-ozE-wKC0EGS2QYJbi0ijGkC6KF0nbIrfCUKD_RpYMDG_GCSyQm4ICEdjIBWNUuzjnR5yaCDUQrs0tVf-32C4WWIm-F6PHae2aQB1X3-Tyb3IHyD2sPpgqwOt6vVlMCEFdjEuZxfovP7EoOXj6_-yKREnwrjLCBBlsHHqnzeRGsLgiV0Rg_YjJVEPSUnqtQCTOAgRl6bhhP6X1uR_b_-SQc0ih5bsDMkqjK1vj4XaOPCAxbHhxjHDzjKfQo4jf9vqcPgqcXlZN=w1050-h548-no)
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

![Do I need more dense mesh?](https://lh3.googleusercontent.com/JHceNBJEhk0yWoHvdSv42DXWUMiLaJiG7rOQrhSd1mFVQBWSCqbGXHcFdhwr0iKN9SfVG2xZ_jsQsnu1ed4Hjyq-aIiLRQLV2612Xeo6_J1_m6QoHBi49ss1CYLSGKdqATGr9Tupr69pcgfKDslhDik8JINC2ANeuOyLyxLXNW2qR0WsQKysslyOO3WCJmLJyTQN7hxxNvbU1s9rVwWiPWIgV5_x7C1tm34HBnmNwhtnng6MVf-igXRcb9sy04dQaFMODk_af7fM-1AR0fIgHMlb02lDFjmhbnbNSL49HKkS2ZlTf-QdyfrDjkeMb24LQzNz5mCBD5DMIBaoHBodV9pAgiGE5QszVU8IQdn18CTNzskzw3-tQntUWFlGTblMBNxH5nvGdYmu5JTEfeSIrHAlO46EgrRGh8dEPEwO_9-wm1XTCCceS1P4Pro1PQHW9I8l4ns_M4UiUkTVqNFXA0qfeb0lG7ZmXs5cOLdraouCPiVZiLyW7idWTLPaOf_TJEJH_AQyDhveD1Eyxy7iGOB0UvR75h2TiSI7o-NtiLdjUZgqyL7uFaQgAUGCYVDBOzFLZFwH6Co9KyQTWZfBHSoi3d24_yWvrjsYlnJ347g7R_eteldLdCIEA4H68647yB4rhy0LPnIbWMbRf9s7H54qhWYZXVdp0Nwp3Ouoaa2B73o1Sqzv7HCj1vYXQQAkWWbqLdxH0q2quAEO=w1050-h572-no)
***
## 2018/12/10
Until today, I did minor fixing/tweaking, and hold camera into world.

And Today, I made UI for individual building. (only UI,not working at now).

Thanks to Screen To Gif tool, now I'll leave more Dynamic pictures.

![Reaction of building UI](https://lh3.googleusercontent.com/54p4QdQDZkYwolmQyNb_5CNw8suvh0qDVCqIpwOkUvHJjlucDe3tUfySRzKR5Rl2qeR_qZPxkg50GfOMLXsVIHoxvqkmON4tu6FMUbBp6SJzEXz1BuFVqxDTM1x0uOlOyaFwPnoMOLCLBfAmA2GlUymAI-zIb-t26zsYorQhKj_7SlDkg8D4ENyx_xbQ0nRfKtq6G7r1fEz9lWTNTWMG4SW2fludFSoo736dtxs3oOMuVu_syecfOdKEOCTSU43FArENshksTv1h93AaZreEy4B9zeA8yvZKq9Xyo9IjR_o3746enjnIC7HyvkMHgTzflVIlpeSE5HaE_1vDtq5krSQjldD57fJfEM3YuFhZZx209SyiNDtFe8Yh_xK2PmglvA4FnYXqzuKmf5uJx5_YqUo2WL2ne7QItHdDVp1wSwz2o1Jb5XIJprBoNlSMYXfO2SUTx9_P6go7QgXBhc5xA6MR9vPUonVTs9t5DZcSlXBASv-NRsbLSw8H5IMbYnI9bptFQygEqa1YAZGTtOHSX9YKO4xup9vphdVrm5y2jw99t7Nb3i35oGDXBWzZUy484UKdDgaroYOxsSh84teBbZXFQeXhfDkY7lrK5zFwQsDXb3Z2nqonRvK_TFYVaplAw5jvN-CrV1Vw-3ugHTRR_8zceajYHNuLhNz1fTFm3yekYfSbFDDs1yNVrnxAQUkbsmVCY-t40el4KjH2=w700-h382-no)

Plus one more!

I felt too bored at studing univ lecture for final exam, so I advanced little more camera/UI things.

Now I can fade in/out any time and make menu for individual building settings(but only object, not script).

![Building Option and fading](https://lh3.googleusercontent.com/WwfKVQNtmeN9vn_pteoSrM80t-huezRFbnOGB-1FHoDHD5FY1OOPRY8Ib94D1zJdyGgqMGppc46ZcEb-lGPvD19HvLzoUosPuaNBdOnJATfDdi2E5-ZhTkwuUMiRsN39TJoiAypWagt_JMYNJSMIGIX7za5sbN5TYmB91SP-bLDdutbiNzSIePAguobKRjge2LpX8kpr_lbXqxKNDc5WWfQmvK5R8PP_EzZJi1kdpp9_Hfh6xNn6xKwg6QRiJXPpdvZjd0YwXY_g1-62liODHmgORy9NSi8zyTqInlGXRJLxcBwk02-DMt6lP4sVWO9CIUf51WygHzpiK-U6vWfDovBZBmkK9a6IogvtKVRgjwKikE2vUmyCIlrJVOnWl3RDVSo56EYyvfxWlwavxeTxtm4_kzNpxKGhvcso4LcZGJ1FCJmDvqE3hhrXDFA7rfCAktgGDWifM27Zc1SbhFP11p6UZhn-6zOVak7f7IV4PD7RJr2TlPH51X4UKzsalQmu1ehTLrl4B8CyjQ14fClJtPUZrfNs5pYA1S2FJexxw4AV8y1CnlvVUEOiIyK34SaE8a2YC_KtmLizSiDOu7IcI30wuCJ2ymq_CIJ8xQy17HSd_cIF5NhmbBCHdbcnkk6Sl9Gvn29OkcBP_hIScH2ggoJXKA_TsZeGc5t2VoQFTEe0SQB6GbqChgkO4ZfI4gp1xRmsD4nwd90zmeU-=w340-h189-no)
***
## 2018/12/19
Finally! Final exam is almost finished.

I'm really, really confused about building up UI structure, so I'm trying many ways.

Until now, I can order units and select units by selecting list in building, but many things can be changed and somethings can be reverted.

For better stuff I built this sutff.

![Does this remains at final build?](https://lh3.googleusercontent.com/cyc3gCF7x-Ji3ESFihny-pF_5ocETYw05na6Z9sjbsAyYk9eMm4kt-v3zWBjmSY4GWKKmpDI4LJkAZhO2xCQ3xX1zfPa1nHdVSwQdXJqcP9kniaJFhe4V8kwI6cNlS8HuIPQ-hBwEcrTAvlYsXs2y5mriViCyMxuiy-FySYkPapnpOicpvv3HPJpKO_wlS83rIJLzAhRYkQHArme1GihgpZexGtORhEGY_VeDmrPNK8IeK6deBCFQPEzyA49HPtF-YSxjGmbaxzeZK9pKRnzVbQvGjgst3Ifbc4rqXm_64QoQznrqccH3iJOTmCi7Mq9uCuhz7Y59_P0YUq1eRPTmkbMZ3EEJIjQycF4MnzON6GZtW7NrRdOrXV-ZXI4k6a4gqJIdCi-kRJ1qBofLgfXYJwoOokXByVQQ7pn-MaiO0tLPzyyV1Qhxm879kjghXB2G1Yd9k7YRCnud0NlpylNOit3X9yy0gTeMQjPgwS9yRD7gCWeUc_o4q_nfqCfOiGkPswdr8qqjLt8rQy9mNK3u2oCUOeekmRjEwZgsgz48KFhiIXedQOgMrLidcW4FbxIYGMdkvfTv7qjpgXKt3Kc-TRaqQtAI3W5Taexxg0LTAJCEtQgPp_hdmTSqv36FKwhVHW-9QqkKAc4W_HyY1fAgNY3qy3Y539zKW_Epx5v-EpX-ifgaJTYFSMorUMv0yuCWYWbODNFTGtr5x6M=w895-h455-no)
***
## 2018/12/23
I made Sun for day/night cycle. (simplified one, can be changed)

And I gave up worldspace Entity Menu because it wasn't great way to show what the entity can do.

Instead of it, I added screen space entity menu with simplified entity information.

I have to add more detailed menu which can show information that not be seen in simplified one.

For now, order command is fine, but have issue in detailed view.

![Time scaled to 10x and screen space located entity menu.](https://lh3.googleusercontent.com/qanyGUuy4k9LJ7ELt-_PaWktDugK3XYHP1o9tMLk6wwlflEKbuPlAnFC3smi4Vu9GohwITVqeFu8Crww_GN3S8OwVriZlhGYeWSEkLdk50RVZA3GNQq8ee6XCBKPAvQ2Os0I20z5lYH9BTU_iFSli70HMlQwAzw0L-G8wIq9kQCN-6eLz9ciQD8bGNZWg3F0dU-xkGrkv_1-wf23pN8rqP4v_r5FryaG8D_mFa0AFyplOzRl8IJP5FglaaUWNUAYouedCjT0UQvKTDyzxOJ4IOsH3riwHhTc25YnNnr9SUQDqfwm80Xlj6o_sK4gZMs8c0v9CJZwpVyCcW2a4x1AH4uBEVG8cwGZeCANGjMIWJlHculk1m96NyIrykxBPJB6N2QZkZadfiMf4YzjVJ-7XIZp8Z36uCGsS2_ajytRoTNl7lPp1G2ULbb5GjHJ_tIkW4OE3mau0f3nwZXY-s3sg-JwhjEGatC5OWWbHtfJehMVwtwrr8yTWgcCMtS0Gk0BdomhOpJkWcZFWRMQM94K-AlkBzSU13uV02a9izL4TwJWReB-LaPj3jGjPrrjynAdc2RN91uyLd5-FaMMdeNRD0tPRJEqbNt3a6OeaZc9Aaft8_uOlNPs6WhmtSKUiUR_d-8rwVdEesYiUb85j8XFI9wBtJdIOJGNnH2opXVzhcQrhOtJ9fJ5EPMMqnFYSXdSAWfiHD7XinKIuHhnjA=w503-h252-no)

... My desktop's mouse wheel is broken.

So zoom input method added with page up/down key.
***
## 2019/01/05
First of all, Happy new Year!

I didn't write any diary since today because it didn't be changed at visualy, but I decided to write this for proving I'm still working.

I'm working now for Items, and interfaces of it.

But using it more wisely, I have to complete day cycle for unit/buildings.
***
## 2019/01/06
I decided to remove item system.

Precisely, I decided to replace item system to resource system.

All individual item is shown by generation speed(per day).

And Total Resource is sum of those individual item's stat.

For example,

```
Carrot earns 2 food resource per one.

World makes 4 carrots per day.

People consume 1 food per day and there are 3 people in world.

There are 1 restaurant and it uses 1 carrots for making 1 carrot soup.

Carrot soup earns 5 food resource per one.

These means :

Carrots : (+4 generation, -1 cooking) 3

Carrot Soups : (+1 carrot soup) 1

Foods : (3*2 carrots, 5*1 carrot soup, -3 people consume) 8

Using carrots more means reducing whole foods.
```
***
## 2019/01/16
A lot of day since I wrote diary.

I tweaked entire structure, and add "Resource".

This can be determined by "How many items generated" and consuming is shown as negative value.

And rebuild entity menu(for now, only in these images).

![Resource using status and camera locking on building](https://lh3.googleusercontent.com/lcyqvjoUuziJBHrz5aDvp3xaB4kwzuceiM84RG_fquMrs0NLOXYotypwbLKVRJ2rM5sVSlfmgt92UeO8aj_2XKSF5y-DO5-SN734sI7DMchgCjyF2Ib2hIeniQyrf27wWVUGReWCWQ8kaz_Pn3dZCvnqCWyWy1XW7-XxUg_OjAAXC1O9elEKsGJ5OSbnoqw2AhGLjsxL1SGmqCZmtnj6iDoE1K8CWUdXKJd4uO-KyMsfiKkS02RQyoL4wakYBxEoqaGKVIyhq7D18w80s8tR8WqEJY0k6Z96JHl8zUIGvjqIgZPNQXBuwW3_nzVw6siGURu2Ew2EsCgJdNjMkd-Bjlh_Ao5q7mqtt8WIATGCeIBGt27KlfNYUfHJO9QAoZw9sFddtWxknDrBJIYgBoc02g115oBojDZfhstosXS4cH9fcwW1yXxvsouezruJMK-j5D7YX0S1iKQTGTpmGUUN2Tqfyix0NUndgxdyNIaX3k1X1axGpIUlg8T6eLqgVfYI4_fmjL5cK-dsPtMVZFa9Ceocgl2egOvnOQrrhAAiNZO8A7dMjcSfKoh4Hc9iejjemol-7yv9UI5hmLwHljgxgJkIcEQcEQh9AxT50ePEpBtkZ71dW3DBtuTWJotSJ9XZCMTkQQPTo0-Lcd7zWwVcDjPiKK2QECQAB9hpwlxfWtrlaEVlpdfi8F2ksdpRS-Ei4eM-VpUyvPBBtvvCUg=w421-h312-no)

![Button syncing when out of workers](https://lh3.googleusercontent.com/ZXP9rhTbZNUnpYExQTYcB_pT_VuJt-T1EVuA6nqrd48cGVJENQ44Xl64F3A6bIr8VpXPStzmgIfp5oINk_wieImmI3xMuBeflI9d_PlQ_sIWK2TcvsZeUAKkXw2KvrPb7YIPZv2k8UOuV_y1cKwJ0z10fQ9kFaFKi-LImpyDEDra5Mg7DJ9Eido-_ARWCig-0FPSA4cDUKu9Rze5uTGv7ZXbeXahIqWrg_Bvk3-imFaRsP4xhi3z2goI2gpXEm9WEFEVodXe2AeKTwnbfSJgVjXXecjtA6VxxIx6to_CPgmcv0_vHaWQYO-zZgtqHtwGXAbBfyyXkVzInbwKhWviG5OJJSxpm_gmZXTy02dnz4KhYJ_J-1-I-6oNtzV7cv1FbDMQh0HWOY_txX9P4I2Bpm_bFyazcbtSZ-sEKRihHndG3ERmFO7z1yzHR7APKOqVfZXidm9J4Ee_AU9brw3JAzNC3FJgZD-tfkoGnxWEwq2R-tK4wbam6JVc7Hm2U0Ol_KTIuZG91H9nedCkQMnEIeY51XEW0lnJIKdrA8NmT0IdnOPj77vRSyURXvsFqUFtTTJopnPIF8QcDlJW_j_859YmhfTIDz36zgRHAVo1REx8QZsNkrbGR8ZMUGuBc-3eru7M4Cb6RW_fM1vkECnwoe3r=w252-h186-no)

...So Yes, Perhaps I can't use building property camera for seeing detailed information. (Actually, I'm not good at asset making... too ugly models what I made-tent/camp)

Until next outshowing changes!

***
## 2019/01/23
A week passed since last comment.

Main menu parts (lower left corner) added and its interfaces.

![Pause menu](https://lh3.googleusercontent.com/6hEYPCRsUHTyHsHLizVGppTD8aamHUDH3ioXpG7lmwLSB_nLHdwv50C8oAqScsWGWK2XJbL4Uwk7Ty2o0s01Y3Xv6ZXwwEk-P8RB7OhUr50x3XTSkiA00vmU4Xu9Rq34XsSkGm_XHloYwNPTYN-dX_y8dRvi4HP2GtpnrPeUAti_ZnAbRQki0R3RVS9zIHyqFmqt8KucPObWBTcsMFjt9eWc-uzHCrSjVLGABC5RuOysgWPvRLn7FSEGL15-xzVa3UxjbLg0i7cYTc8zAcabOVDBj2D-R5E6azyAqVzUXmAQEOVQHZ8JfIV5DJGF9-ag1hdq_H4tj9oD9rfH0mw6YJlkRGaFsXRjSgY8U2uggu8v0Y4p3_d93fbDLNCSJ939DGm5nbdogCFIDO-aMVHs8UFfb7vZnw6DSw4iYiD2whD_u9dBSNrC4Lx4eL3ExxjT51lRTFmTygFuR1gSVxXPDj-nKAnmbdN8ImLR2W_xisD8f-FL2t-J4sB_a5H2F-83yP3hq9bnFVU-yc73Oc9t50wIgvwLr2USkuwfrWC4KoM_5fpd7uXR8wmCELPlEREw7IH9O4_lONpEi-hHn36SvRSR6V0pkBk2_3ZtWWDp1zDBsfydprF8O_8LhEXkHgE-yY0NiU3UeJWrFk1XdAfnD8wyc3aJCxk9jAY8Y4JvU4DtNdB5duarNEVhtu7unq2Wch0e0hn3l-65KeLHXQ=w570-h318-no)

This really pauses time!

![Resource menu](https://lh3.googleusercontent.com/QsoWO2qe1M38aNEkkjXV22maAtrBq2XVb_WWhncA1JMm21x5EufpftLpK-wyM49coRtKlx2AtbGTpIIb6al0qyMb7-q-sm2zTL1otRZv2GTRUNZjQwp0qb8l5Mv83YII5_Vndp9c75BWXeDf5BAnbh9IVFiqK1NcoynhYQ_2hDpoMBHW3uGWqOq5kX-_WPRKTcY7jziEgqJDaSouQKqFATwomHTg-eLtNWWXkETt0NAhQa-3Z0Du3A64Tj4T9D7xXqz_7TOemvTEhpqgA_uztU-NojLTPSQjgzvBr3Y9EMWMqU-MPslmr6N7msrZJNGHpq8MhxoY0XKkF-UMkp63QRQfMQlNG7nxCc6DZE4R8e4_2bEgS6zTV9HOYxNOqAy4_-XXlDbJCUI263YNBmyMBM-atM_YY3hGVFCcDDLyEipd0JFAQs-Jdh-LO7vkrZXoydEl-IwMv-pOeSpE9rv7f6qrFNnnq_2Yl4puBLo5kW9i7sl06RiCT1M7zln5K1TPNmw0SFL87KdyRyIZXxptN3Z47aiw1bZ046kAR7VrppYE_QwB--EPckSmIfLMb4F-pwWLmVmAa9UgJFaWzfae3Eqflp09UVjsh2Di9EizhMLulaaEmRzAEnjTB5ZzQlFd3LnoXfHkT6d4L9y_txK2Zy7hgu9zJ1KXYEWnyGXdpV0pCMdJTj9xKI5ZxXKQLQ3EQy4MhUZOdEa9UqbH_w=w1692-h955-no)

This shows entire resource production right now.

***
## 2019/01/29
I'm gonna little busy these days.

I'm metoring my club's young members with advancing other project.

I coroutinized loading for responsive loading screen.

And this is what actual 'loading' doing : 

![loading sequence](https://lh3.googleusercontent.com/PO48ImXXzObU4-45DD_T0m5fhLa8IEFwWoqgShpMNGcmTVIo64sUpJBEXAUmQ_kVxq2u04A-YLHf9wmKgHzvIDqtGImlFhuVZRwCrfbGsBBbmAJtMqdWgYDBrrsVMR7wtHxN2Ex1nLFRe96IPsRudsSGRtsjxzfnuUztE8p37CeAcEjijIB_B_9PkKfmioWPniNIG72sp0PsbOYmiUK15PB4rWx-76pSvC95rjfeKrhmPVDE0Fojdk7qpFnexx0RLvqbAySn3mq_5xdUN36yTxz1JPxHeU2742ftRx9pcOZnitvketaC27FA_qPUgoBHSv8h9RfemQhuyUwxTZ_vKULDVN-TjWBW_O94_cMz-hEixOqarZhc5BHynITVjedpknvLvZyD3rftP442CdTI1zHqaEHc0i1ksYJqREnHx3t1Vf_n6NT3QNEDnoRhSrGDLRIUu8ZhowznJEwAOD9X0zWsmWiCx77BHM-VEZ411gHrPJLpk4S8NL-GsAyVTnsJdGmibTXJdviI2hqG0pEKMJfKT_MjG9_7DNn352bFUsYRThZYnvfH-ccTaRgCV-3Gwjjug0lEtqUnp7UjVaiekRWEBhJkZkCn3c0sgN2pEyT2QSKzwh-nKDYQT8dVtPMb8b2K5JA_6voaHjv7kd8LYtGtEKRTTFIztzqrrAhi_WHBSKd5J-_9s_dfRqTd_U5YHaQGbshGqVTL8O20pA=w300-h183-no)