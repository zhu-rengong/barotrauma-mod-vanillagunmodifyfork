# Q&A

## Modding

### Q: 为什么配件插入枪械时调用的`StatusEffect { ActionType: OnInserted }`需要延迟执行？
```xml
<!-- [槽位0] 枪口 -->
<Containable identifier="VGM_LongSuppressorMuzzle" hide="false" itempos="49,2">
    <StatusEffect type="OnInserted" target="This" targetitemcomponent="RangedWeapon" weapondamagemodifier="1.2" barrelpos="150,0" setvalue="true" delay=".016" />
</Containable>
```
**A:** 玩家通过拖拽调换物品时，从内到外和从外到内的 OnInserted & OnRemoved 的调用顺序不同，为确保OnInserted始终在OnRemoved之后调用，需要添加个delay

### Q: 为什么有些配件要用`StatusEffect { ActionType: OnContaining }`修改武器，而不是使用`OnInserted`？
**A:** `StatusEffect { ActionType: OnInserted }`无法在加载地图时调用，导致无法序列化的属性将被重置，例如`RangedWeapon.BarrelPos`

### Q: 扫描器、瞄具、握把不都是用于修改枪械的吗？但为何修改的方式弯弯绕绕且有所不同？
**A:** **其一**，需要判断枪械配件的改装结果是否具有不变的特性，对于瞄具、扫描器，无论插入到何种枪械，对于枪械的变化都是相同的，适合采取高度耦合的修改方案提高代码复用并减轻其臃肿程度；而对于握把，要根据枪械结构的实际情况来决定其握持方式。**其二**，部分需要修改的组件属性是无法序列化的，比如扫描器，它的修改结果无法应用到下一个巡回加载时。
