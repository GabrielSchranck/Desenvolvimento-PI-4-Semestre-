export class Card {
  id: number;
  title: string;
  description: string;
  image: string;
  price: string;
  author: string;
  rating: number;
  available: boolean;
  category?: string;
  location?: string;

  constructor(
      id: number,
      title: string,
      description: string,
      image: string,
      price: string,
      author: string,
      rating: number,
      available: boolean,
      category?: string,
      location?: string
  ) {
      this.id = id;
      this.title = title;
      this.description = description;
      this.image = image;
      this.price = price;
      this.author = author;
      this.rating = rating;
      this.available = available;
      this.category = category;
      this.location = location;
  }


  static formatPrice(value: number): string {
      return `R$ ${value.toFixed(2).replace('.', ',')}`;
  }
}
