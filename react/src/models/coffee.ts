export type Acidity = 'low' | 'medium' | 'high';
export type Roast = 'light' | 'medium' | 'dark';

export interface Coffee {
  id: string;
  name: string;
  origin: string;
  tastingNotes: string;
  bitterness: number;
  acidity: Acidity;
  body: number;
  roast: Roast;
  bestFor: string;
}
