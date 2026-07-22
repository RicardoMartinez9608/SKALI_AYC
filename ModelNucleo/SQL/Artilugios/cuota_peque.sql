use aycnucleo_alpha;
delimiter $$
create function Cuota(
		monto decimal(18,2),
		numero_cuota decimal(18,2),
		interes_peque decimal(18,2),
        plazo_mes decimal(18,2)
    ) returns decimal(18,2) DETERMINISTIC
    begin
    
		declare tasa_centecima decimal(65,30) default 0.00;
        declare potencia decimal(65,30) default 0.00;
        declare cuota decimal(65,30) default 0.00;
        set tasa_centecima=(interes_peque/100)/(365/(30/(numero_cuota/plazo_mes)));
		set potencia=pow((1+tasa_centecima),numero_cuota);/*el signo negativo hay que cambiarlo si se usa otra formula*/
        /*set cuota=monto*(tasa_centecima/(1-(pow((1+tasa_centecima),-numero_cuota))));*/
        
        set cuota=COALESCE(round(monto*((potencia*tasa_centecima)/(potencia-1)),2),0.00);
        return cuota;
    end $$