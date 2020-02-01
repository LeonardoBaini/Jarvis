package MetodosSql;
 
import java.awt.Desktop;
import java.io.BufferedReader;
import java.io.DataInputStream;
import java.io.File;

import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Statement;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.Locale;



 
public class MetodosSql extends Conexion {
	
	

     
    public MetodosSql() {
    	
    }  
   
     
         
    public static String dameFechaDeHoy(){
         SimpleDateFormat formateador = new SimpleDateFormat("yyyy'-'MM'-'dd", new Locale("es_ES"));
         Date fechaDate = new Date();
          String fecha=formateador.format(fechaDate);
           
     
    return fecha;
    }
     
    public String dameFechaDeHoyConFormatoX(String formatoFechaseparadoXguionyGuionEntreComillas){//el MM va con mayuscula
         SimpleDateFormat formateador = new SimpleDateFormat(formatoFechaseparadoXguionyGuionEntreComillas, new Locale("es_ES"));
         Date fechaDate = new Date();
         String fecha=formateador.format(fechaDate);
          
     
    return fecha;
    }
    /**
     * 
     * @param sentenciaSql
     * @return Devuelve la cantidad de filas ejecutadas por el update, si devuelve -1, error, sino ok.
     */
    public int hacerUpdate(String sentenciaSql) {
    	System.out.println("Intentando hacer "+sentenciaSql);
    	Connection conn=new Conexion().conectar();
    	int errorCode=-1;
    	try {
			Statement stmt  = conn.createStatement();
			errorCode=stmt.executeUpdate(sentenciaSql);
		} catch (SQLException e) {
			System.out.println(e.getMessage());
		
		}  
    	
    	return errorCode;
    }
 
 
    public ArrayList<ArrayList<String>> consultar(String SentenciaSql) {
    	
    	Connection conn=null;
        ArrayList<ArrayList<String>> matriz = new ArrayList<ArrayList<String>>();//creo una matriz
        String aux=null;
         
        Conexion con = new Conexion();
         
         
        try {
        	
            conn=con.conectar();
            Statement stmt  = conn.createStatement();  
               
            ResultSet rs=stmt.executeQuery(SentenciaSql);
          
            ResultSetMetaData rmd = rs.getMetaData(); //guardo los datos referentes al resultset
             
              
                while ( rs.next()){
                        ArrayList<String> columnas = new ArrayList<String>();
                         for (int i=1; i<=rmd.getColumnCount(); i++) {
                             aux=rs.getString(i);            
                                  
                             columnas.add(aux);
                            
                         }
                         matriz.add(columnas);
                         System.out.println(columnas);
                }
            con.desconectar();
 
             
 
        } catch (Exception e) {
            System.out.println("Error en metodosSql.consultar"+e.getMessage());
            System.out.println(e.getLocalizedMessage());
          
             
        }
 
        return matriz;
         
 
    }

 public ArrayList<String> consultarUnaColumna(String SentenciaSql) {
    	
    	Connection conn=null;
        ArrayList<String> lista = new ArrayList<String>();
                 
        Conexion con = new Conexion();
         
         
        try {
        	
            conn=con.conectar();
            Statement stmt  = conn.createStatement();  
               
            ResultSet rs=stmt.executeQuery(SentenciaSql);
          
         
             
              
                while ( rs.next()){
                       
                        lista.add(rs.getString(1));
                        
                }
            con.desconectar();
 
             
 
        } catch (Exception e) {
            System.out.println("Error en metodosSql.consultar"+e.getMessage());
            System.out.println(e.getLocalizedMessage());
          
             
        }
 
        return lista;
         
 
    }


public String consultarLinea(String SentenciaSql) {
	
	Connection conn=null;
    String linea = "Ejecutable";
    String aux=null;
     
    Conexion con = new Conexion();
     
     
    try {
    	
        conn=con.conectar();
        Statement stmt  = conn.createStatement();  
           
        ResultSet rs=stmt.executeQuery(SentenciaSql);
      
        ResultSetMetaData rmd = rs.getMetaData(); //guardo los datos referentes al resultset
         
          
            while ( rs.next()){
                    
                     for (int i=1; i<=rmd.getColumnCount(); i++) {
                         aux=rs.getString(i);            
                              
                         linea=linea+" "+aux;
                        
                     }
                   
            }
        con.desconectar();

         

    } catch (Exception e) {
        System.out.println("Error en metodosSql.consultar"+e.getMessage());
        System.out.println(e.getLocalizedMessage());
      
         
    }

    return linea;
     

}
     
         
         


	public String consultarUnaCelda(String SentenciaSql) {
		Connection conn=null;      
        Conexion con = new Conexion();
        ArrayList<String> arreglo = new ArrayList<String>();//creo una matriz
         
           
        try {
            conn=con.conectar();
            Statement stmt  = conn.createStatement();  
               
            ResultSet rs=stmt.executeQuery(SentenciaSql);
          
               
                while ( rs.next()){
                     
                    arreglo.add(rs.getString(1));
                }
            con.desconectar();
 
            }
 
         catch (Exception e) {    
          System.out.println(e.getMessage());
             
        }
        if(arreglo.isEmpty()) {
        	return "";
        	
        }else {
        	  return arreglo.get(0);
        }
      
         
 
    }
   
         
      
   
    public String LeeArchivoParametros(String archivo)  {
    	
    	  String resultado=null;    
    	  String strLinea=null;
    	  InputStream fstream = this.getClass().getResourceAsStream(archivo);
          // Creamos el objeto de entrada
          DataInputStream entrada = new DataInputStream(fstream);
          // Creamos el Buffer de Lectura
          BufferedReader buffer = new BufferedReader(new InputStreamReader(entrada)); 
          
          // Leer el archivo linea por linea
          try {
			while ((strLinea = buffer.readLine()) != null)   {
			      // Imprimimos la línea por pantalla
				  if(resultado==null){
			    	  resultado=strLinea;
			         }else{
			        	 resultado=resultado+" "+strLinea;
			         }
			  
			   
			     // System.out.println(strLinea);
				 
			  }
		
		} catch (IOException e) {
			System.out.println(e.getMessage());
		//	e.printStackTrace();
			
		}
          // Cerramos el archivo
          try {
			entrada.close();
		} catch (IOException e) {
			System.out.println(e.getMessage());
			//e.printStackTrace();
			
		}    			
          return resultado;
   	}   	
    
    public ArrayList<String> LeeArchivoParametrosArray(String archivo)  {
    	ArrayList<String>listaParam=new ArrayList<String>();
  	 
  	  String resultado=null;    
  	  String strLinea=null;
  	  InputStream fstream = this.getClass().getResourceAsStream(archivo);
        // Creamos el objeto de entrada
        DataInputStream entrada = new DataInputStream(fstream);
        // Creamos el Buffer de Lectura
        BufferedReader buffer = new BufferedReader(new InputStreamReader(entrada)); 
        
        // Leer el archivo linea por linea
        try {
			while ((strLinea = buffer.readLine()) != null)   {
			      // Imprimimos la línea por pantalla
				  if(resultado==null){
			    	  resultado=strLinea;
			         }else{
			        	 resultado=resultado+" "+strLinea;
			         }
			  
			   
				  listaParam.add(strLinea);
				 
			  }
			
		} catch (IOException e) {
			System.out.println(e.getMessage());
		//	e.printStackTrace();
			
		}
        // Cerramos el archivo
        try {
			entrada.close();
		} catch (IOException e) {
			System.out.println(e.getMessage());
			//e.printStackTrace();
			
			
		}    			
        return listaParam;
 	}   	
	       
	      
		
	
    public void abrirarchivo(String archivo){

        try {
        	
        	 
               File objetofile = new File (archivo);
               Desktop.getDesktop().open(objetofile);
             
               

        }catch (IOException ex) {
        //System.out.println(ex.getMessage());
      
        }
        
    }
    
    


 
    public boolean existeArchivo(String rutaAlArchivo){
    	File af = new File(rutaAlArchivo);
    	if(af.exists()) {
    		return true;
    	}else {
    		return false;
    	}
		
 	}
 
     
   
     
 
}