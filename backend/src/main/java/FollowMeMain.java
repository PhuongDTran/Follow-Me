import static spark.Spark.*;
import static spark.Spark.get;

import com.followme.groupid.GroupIdController;
import com.followme.util.Path;

public class FollowMeMain {

	public static void main(String[] args) {
		get("/hello/", (req,res) -> "hello");
		
		get(Path.Web.GROUPID,   GroupIdController.HandleGroupIdRequest);

	}

}
