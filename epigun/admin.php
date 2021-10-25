<?php

session_start();

ini_set('display_errors',1);

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
$sq = $mysql_connection->prepare("SELECT content FROM config WHERE name='sitename' OR name='admin_password' ORDER BY id");
$sq->execute();
$q=$sq->fetchAll();
$sitename=$q[0]["content"];

if($q[1]["content"] != ""){
	if(!isset($_SESSION["admin"])){
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
				$_SESSION["admin"] = 1;
			}else{
			?>
				Try again
<?php
				die;
			}
		}
	}
}


if(isset($_GET["on"])){
	$q=$mysql_connection->prepare("UPDATE pages SET online=1 WHERE id=:id");
	$q->execute(["id"=>$_GET["on"]]);
}

if(isset($_GET["off"])){
	$q=$mysql_connection->prepare("UPDATE pages SET online=0 WHERE id=:id");
	$q->execute(["id"=>$_GET["off"]]);
}


if(isset($_GET["delete"])){
	$q=$mysql_connection->prepare("DELETE FROM pages WHERE id=:id");
	$q->execute(["id"=>$_GET["delete"]]);
}

if(isset($_GET["goedit"])){
	$q=$mysql_connection->prepare("UPDATE pages SET title=:title, content=:content WHERE id=:id");
	$q->execute(["id"=>$_GET["goedit"],
		"title" => $_POST["title"],
		"content" => $_POST["content"]]);
}
if(isset($_GET["newpage"])){
	$q=$mysql_connection->prepare("INSERT INTO pages (online,menu,title,content) VALUES (0,2,:title,:content)");
	$q->execute(["title" => $_POST["title"],
		"content" => $_POST["content"]]);
}

if(isset($_GET["edit"])){
//On cherche la page
$q=$mysql_connection->prepare("SELECT * FROM pages WHERE id=:id");
$q->execute(["id"=>$_GET["edit"]]);
$pages=$q->fetchAll();
}else{
//On cherche la page
$q=$mysql_connection->prepare("SELECT * FROM pages");
$q->execute();
$pages=$q->fetchAll();
}
?>
<!-- TinyCMS by j972 -->
<!doctype html>
<html lang="en">
  <head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <title><?php echo $sitename; ?></title>

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

<script src="https://cdn.tiny.cloud/1/1tgp1axqba0ydjjo3ynj33kzfk3aeuvfflfrgrjjxqdwy5w0/tinymce/5/tinymce.min.js" referrerpolicy="origin"></script>

<script>
    tinymce.init({
      selector: '#textarea'
    });
  </script>
    
  </head>
  <body>
    
<nav class="navbar navbar-expand-md navbar-dark fixed-top bg-dark">
  <div class="container-fluid">
  <a class="navbar-brand" href="./admin.php"><?php echo $sitename; ?></a>
    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarCollapse" aria-controls="navbarCollapse" aria-expanded="false" aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarCollapse">
    <ul class="navbar-nav me-auto mb-2 mb-md-0">
        <li class="nav-item">
	<a class="nav-link"></a>
        </li>
      </ul>
    </div>
  </div>
</nav>

<main class="container">
  <div class="bg-light p-5 rounded">
  <h1>Admin - Pages</h1>
	<BR><a href="admin.php?new">New page</a>
	<BR>
	<ul>
		<?php
			
	if(isset($_GET["edit"])){$page=$pages[0];
?>
	<form action="admin.php?goedit=<?php echo $page["id"]; ?>" method="post">
		<input type="text" name="title" value="<?php echo $page["title"]; ?>"><br>
		<textarea id="textarea" name="content"><?php echo $page["content"]; ?></textarea><br>
		<input type="submit" value="Go">
		</form>
<?php
	}else if(isset($_GET["new"])){
?>
	<form action="admin.php?newpage" method="post">
		<input type="text" name="title"><br>
		<textarea id="textarea" name="content"></textarea><br>
		<input type="submit" value="Go">
		</form>
<?php
	
	}else{
foreach($pages as $page){
?>
				<li>
<?php
				echo $page["title"];
				echo " – <a href='admin.php?edit=".$page["id"]."'>Edit</a>";
				if($page["online"]<2){
				echo " – <a href='admin.php?delete=".$page["id"]."'>Delete</a>";
				if($page["online"]==1){
					echo " – <a href='admin.php?off=".$page["id"]."'>Put offline</a>";
				}else{
					echo " – <a href='admin.php?on=".$page["id"]."'>Put online</a>";
				}
				}
?>
				</li>
<?php
			}}

		?>
	</ul>	

  </div>
</main>



    <script src="./bootstrap-5.1.3-dist/js/bootstrap.bundle.min.js" integrity="sha384-ka7Sk0Gln4gmtz2MlQnikT1wXgYsOg+OMhuP+IlRH9sENBO0LRn5q+8nbTov4+1p" crossorigin="anonymous"></script>

      
  </body>
</html>
