# 枪口火焰定位有误
- 在不装配枪口配件时，枪口火焰的定位原点则在gun本身，但`StatusEffect.Offset`特性不会考虑实体角度，因此产生的火焰粒子总会存在些微的位置误差，需等官方采纳https://github.com/FakeFishGames/Barotrauma/discussions/16603进行解决
- 在不装配枪口配件时，若将枪口火焰的原点定位在内容物上，虽能做到理论无误差，但次要容器会混淆需求检查，具体请参考https://github.com/FakeFishGames/Barotrauma/discussions/15923

# 收束器的第一颗弹丸散布没有改变
目前采用的是个人认为最具兼容性的写法，但底层代码发力，仍然绕不开甲鱼严格的时序控制，因为在检测到霰弹枪弹药发射时，它的调用始终发生在第一颗弹丸hitscan之后。