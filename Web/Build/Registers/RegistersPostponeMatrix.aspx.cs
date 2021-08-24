using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Registers_RegistersPostponeMatrix : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }   
    protected void gvPostponematrix_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            DataTable dt = PhoenixRegistersPostponementMatrix.List();
            gvPostponematrix.DataSource = dt;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Postponematrix_TabStripCommand(object sender, EventArgs e)
    {
    }
    
    protected void gvPostponematrix_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }
    protected void gvPostponematrix_ItemDataBound(Object sender, GridItemEventArgs e)
    {

    }
    protected void gvPostponematrix_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }
}