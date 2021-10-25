<?php

session_start();

//ini_set('display_errors',1);

/********************************************/
// MySQL
try{
	$mysql_connection = new PDO('mysql:host=localhost;dbname=Épigun;charset=utf8','root','root');
}catch(Exception $e){
	die('Erreur : '.$e->getMessage());
}
/********************************************/


if(!isset($_GET["id"])){
	$q=$mysql_connection->prepare("SELECT content FROM config WHERE name='home'");
	$q->execute();
	$id=$q->fetchAll();
	$id=$id[0]["content"];
}else{
	$id = htmlentities($_GET["id"]);
}

//On récupère la config
$sq = $mysql_connection->prepare("SELECT content FROM config WHERE name='sitename' OR name='visitor_password' ORDER BY id");
$sq->execute();
$q=$sq->fetchAll();
$sitename=$q[0]["content"];

if($q[1]["content"] != ""){
	if(!isset($_SESSION["visitor"])){
	       if(!isset($_POST["pwd"])){
		?>Please enter the visitor’s password and press enter: 
			<form method="post">
				<input type="password" name="pwd">
			</form>
			</div>
<?php
		die();
		}else{
			if(htmlspecialchars($_POST["pwd"]) == $q[1]["content"]){
				$_SESSION["visitor"] = 1;
			}else{
			?>
				Try again
<?php
				die();
			}
		}
	}
}



//On cherche la page
$q=$mysql_connection->prepare("SELECT * FROM pages WHERE id=:id AND online=1 OR online=3 ORDER BY online");
$q->execute(['id'=>$id]);
$page=$q->fetchAll();
$page=$page[0];

//menu
$q=$mysql_connection->prepare("SELECT id,title FROM pages WHERE online=1 OR online=2 ORDER BY menu");
$q->execute(['id'=>$id]);
$menu=$q->fetchAll();


?>
<!-- TinyCMS by j972 -->
<!doctype html>
<html lang="en">
  <head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <title><?php echo $sitename." – ".$page["title"]; ?></title>

    <!-- Bootstrap core CSS -->
<link href="./bootstrap-5.1.3-dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-1BmE4kWBq78iYhFldvKuhfTAU6auU8tT94WrHftjDbrCEXSU1oBoqyl2QvZ6jIW3" crossorigin="anonymous">

    <style>
      .bd-placeholder-img {
        font-size: 1.125rem;
        text-anchor: middle;
        -webkit-user-select: none;
        -moz-user-select: none;
        user-select: none;
      }

      @media (min-width: 768px) {
        .bd-placeholder-img-lg {
          font-size: 3.5rem;
        }
      }

body {
  /*min-height: 75rem;*/
  padding-top: 4.5rem;
}
    </style>

    
  </head>
  <body>
    
<nav class="navbar navbar-expand-md navbar-dark fixed-top bg-dark">
  <div class="container-fluid">
  <a class="navbar-brand" href="./"><?php echo $sitename; ?></a>
    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarCollapse" aria-controls="navbarCollapse" aria-expanded="false" aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarCollapse">
    <ul class="navbar-nav me-auto mb-2 mb-md-0">
    <?php foreach($menu as $item){ ?>
        <li class="nav-item">
	<a class="nav-link <?php if($id==$item["id"]){?>active<?php } ?>" aria-current="page" href="./?id=<?php echo $item["id"]; ?>"><?php echo $item["title"]; ?></a>
        </li>
    <?php } ?>
      </ul>
    </div>
  </div>
</nav>

<main class="container">
  <div class="bg-light p-5 rounded">
  <h1><?php echo $page["title"]; ?></h1>
	
  <?php echo $page["content"]; ?>

  </div>
</main>



    <script src="./bootstrap-5.1.3-dist/js/bootstrap.bundle.min.js" integrity="sha384-ka7Sk0Gln4gmtz2MlQnikT1wXgYsOg+OMhuP+IlRH9sENBO0LRn5q+8nbTov4+1p" crossorigin="anonymous"></script>

      
  </body>
</html>
