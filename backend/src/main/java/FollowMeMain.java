import static spark.Spark.*;
import static spark.Spark.get;

public class FollowMeMain {

	public static void main(String[] args) {
		get("/hello/", (req,res) -> "hello");

	}

}
