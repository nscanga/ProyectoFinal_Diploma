-- Script para crear la tabla DigitosVerificadores
-- Esta tabla almacena los c�digos de verificaci�n digital (DVH) para cada registro

CREATE TABLE DigitosVerificadores (
    Id int IDENTITY(1,1) PRIMARY KEY,
    IdRegistro uniqueidentifier NOT NULL,
    NombreTabla nvarchar(100) NOT NULL,
    DVH nvarchar(256) NOT NULL,
    FechaModificacion datetime NOT NULL DEFAULT GETDATE(),
    
    -- Crear �ndice �nico compuesto para evitar duplicados
    CONSTRAINT IX_DigitosVerificadores_Unique UNIQUE (IdRegistro, NombreTabla)
);

-- Crear �ndices para mejorar el rendimiento
CREATE INDEX IX_DigitosVerificadores_IdRegistro ON DigitosVerificadores (IdRegistro);
CREATE INDEX IX_DigitosVerificadores_NombreTabla ON DigitosVerificadores (NombreTabla);
CREATE INDEX IX_DigitosVerificadores_FechaModificacion ON DigitosVerificadores (FechaModificacion);

-- Comentarios para documentar la tabla
EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Tabla que almacena los c�digos de verificaci�n digital (DVH) para integridad de datos',
    @level0type = N'SCHEMA', 
    @level0name = N'dbo', 
    @level1type = N'TABLE', 
    @level1name = N'DigitosVerificadores';

EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'GUID del registro en la tabla referenciada',
    @level0type = N'SCHEMA', 
    @level0name = N'dbo', 
    @level1type = N'TABLE', 
    @level1name = N'DigitosVerificadores',
    @level2type = N'COLUMN', 
    @level2name = N'IdRegistro';

EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Nombre de la tabla a la que pertenece el registro',
    @level0type = N'SCHEMA', 
    @level0name = N'dbo', 
    @level1type = N'TABLE', 
    @level1name = N'DigitosVerificadores',
    @level2type = N'COLUMN', 
    @level2name = N'NombreTabla';

EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'C�digo de verificaci�n digital hash calculado',
    @level0type = N'SCHEMA', 
    @level0name = N'dbo', 
    @level1type = N'TABLE', 
    @level1name = N'DigitosVerificadores',
    @level2type = N'COLUMN', 
    @level2name = N'DVH';

EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Fecha y hora de la �ltima modificaci�n del DVH',
    @level0type = N'SCHEMA', 
    @level0name = N'dbo', 
    @level1type = N'TABLE', 
    @level1name = N'DigitosVerificadores',
    @level2type = N'COLUMN', 
    @level2name = N'FechaModificacion';