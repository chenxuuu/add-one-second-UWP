<?php
$html = file_get_contents('https://angry.im/l/life');
$allSeconds = intval($html);
echo $allSeconds."<br/>";
if($allSeconds > 10)
{
  $counterFile = "second.txt";

if (!file_exists($counterFile))
{
	echo "read fail";
}
else
{
file_put_contents($counterFile, $allSeconds);
  //echo file_get_contents($counterFile);
}
}
?>
