package MetodosSql;
 


import java.sql.*;

 
 
 
public class Conexion {
     
 
        private  Connection c;
           
        private static String url = "jdbc:sqlite:C:/Users/LEO BAINI/Desktop/Jarvis/Sqlite/data.db";
    
		public Conexion(){
             
        }
         
		 public Connection conectar() {
		       
		        try {
		            // db parameters
		           
		            // create a connection to the database
		            c = DriverManager.getConnection(url);
		            
		           // System.out.println("Connection to SQLite has been established.");
		            
		        } catch (SQLException e) {
		            System.out.println(e.getMessage());
		        } 
				return c;
		    }
             
        public  void desconectar(){
           
             
                try {
                    if (c != null){
                        c.close();                      
                         
                 
                    }
                    else{
                        System.out.println("Ya estaba desconectado");
                         
                    }
                     
                   
                } catch (SQLException e) {
                    // TODO Auto-generated catch block
                	
                    System.out.println("Desconectado incorrecto");
                    e.printStackTrace();
                }
                 
             
        }
}