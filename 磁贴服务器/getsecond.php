<?php
$html = file_get_contents('https://angry.im/l/life');
$allSeconds = intval($html);
$dd = $allSeconds / 60 / 60 / 24 % 9999999;
$hh = $allSeconds / 60 / 60 % 24;
$mm = $allSeconds / 60 % 60;
if(date("m/d")=="08/17"){$result = 75;}else{$result = rand(1,74);}
?>
<tile>
  <visual version="2">
    <binding template="TileSquare150x150PeekImageAndText01" fallback="TileSquarePeekImageAndText01">
      <image id="1" src="http://open.papapoi.com/excited/<?php echo $result;?>.png" alt="alt text"/>
      <text id="1">时间众筹总计</text>
      <text id="2"><?php echo $dd.'天';?></text>
      <text id="3"><?php echo $hh.'小时';?></text>
      <text id="4"><?php echo $mm.'分钟';?></text>
    </binding>  
  </visual>
</tile>