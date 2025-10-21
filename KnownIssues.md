# 枪口火焰定位有误
- 在不装配枪口配件时，枪口火焰的定位原点则在gun本身，但`StatusEffect.Offset`特性不会考虑实体角度，因此产生的火焰粒子总会存在些微的位置误差，需等官方采纳https://github.com/FakeFishGames/Barotrauma/discussions/16603进行解决
- 在不装配枪口配件时，若将枪口火焰的原点定位在内容物上，虽能做到理论无误差，但次要容器会混淆需求检查，具体请参考https://github.com/FakeFishGames/Barotrauma/discussions/15923
