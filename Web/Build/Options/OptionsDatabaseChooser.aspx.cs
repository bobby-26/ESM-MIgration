using System;
using System.IO;
using System.Xml;
using System.Data;
using SouthNests.Phoenix.Framework;

public partial class OptionsDatabaseChooser : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string phoenixdatabasexml = Server.MapPath("~").TrimEnd('\\') + "\\PhoenixDataBase.xml";

        if (File.Exists(phoenixdatabasexml))
        {
            DataSet ds = new DataSet();
            ds.ReadXml(phoenixdatabasexml);
            ddlDatabase.DataSource = ds;
            ddlDatabase.DataTextField = "DBNAME";
            ddlDatabase.DataValueField = "DBVALUE";
            ddlDatabase.DataBind();

            if (!IsPostBack)
            {
                if (Session["CURRENTDATABASE"] != null)
                    ddlDatabase.SelectedValue = Session["CURRENTDATABASE"].ToString();
                else
                    ddlDatabase.SelectedValue = "eLogWeb";
            }
        }
    }

    protected void btnChooseDatabase_Click(object sender, EventArgs e)
    {
        if (ddlDatabase.SelectedValue.ToString().ToUpper().Equals("ELOGWEB"))
            Session["CURRENTDATABASE"] = null;
        else
            Session["CURRENTDATABASE"] = ddlDatabase.SelectedValue;
    }
}
