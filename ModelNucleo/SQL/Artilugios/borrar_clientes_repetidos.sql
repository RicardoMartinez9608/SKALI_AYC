use administracion;
delete from administracion.maeasoc where locate('F',CODIGO,1)=1;

delete FROM administracion.maeasoc
WHERE
	CODIGO IN(
		SELECT 
			CODIGO
		FROM 
        (
			SELECT 
				CODIGO, 
                row_number() OVER
                (
					partition by 
						DUI 
					ORDER BY 
						DUI
				) AS ROW_NUM FROM administracion.maeasoc) T 
					WHERE 
						ROW_NUM>1 and 
                        locate('F',CODIGO,1)=1 and 
                        DUI!="" and DUI !="-" 
                        and DUI !="00000000-0" and DUI !="00000000-1");